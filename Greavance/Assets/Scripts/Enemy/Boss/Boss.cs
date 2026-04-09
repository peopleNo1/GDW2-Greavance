using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    Rigidbody2D _rb;
    protected PhaseManager phaseManager;
    protected BossBasicAttacks bossBasicAttacks;
    [SerializeField] protected Slider slider;
    [SerializeField] protected Text text;

    public Transform _playerPos;

    public bool isActing = false;
    public bool isCasting = false;
    public float actionCooldown = 5f;
    public float nextActionTime = 0f;

    [Header("Base Enemy Stats")]
    public float _maxHealth = 500.0f;
    public float _currentHealth;

    [Header("Animation")]
    [SerializeField] public Animator _animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BossSpriteMovement _spriteMovement;

    [Header("Basic Attack Settings")]
    [SerializeField] private int _basicAttackUsed = 0;
    private int _basicAttackToUse = 0;
    [SerializeField] private GameObject _basicAttackPrefab;
    [SerializeField] private float _basicAttackCooldown = 0.5f;
    [SerializeField] private Transform _basicAttackFirePoint;

    //Patrol settings
    [Header("Patrol Settings")]
    private Transform _currentMovingTarget;
    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;
    [SerializeField] private float _movingSpeed = 8f;

    private int reachedTarget = 0; // A count so that the boss doesnt flip when phase 2 starts

    protected void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        phaseManager = GetComponent<PhaseManager>();
        bossBasicAttacks = GetComponent<BossBasicAttacks>();
        _spriteMovement = GetComponent<BossSpriteMovement>();
    }

    public void Update()
    {
        CheckIfDead();

        if (!phaseManager.isPhase2)
        {
            UpdatePhase1();
        }
        else
        {
            UpdatePhase2();
        }
    }

    private void Start()
    {
        _currentMovingTarget = _pointB;

        nextActionTime = Time.time;

        ResetHealth();
    }

    private void ResetHealth()
    {
        _currentHealth = _maxHealth;
        slider.maxValue = _maxHealth;
        slider.value = _maxHealth;
    }

    public void TakeDamage(float damage)
    {
        _animator.SetTrigger("WasDamaged");

        _currentHealth -= damage;
        slider.value -= damage;
        text.text = _currentHealth.ToString() + "/" + _maxHealth;
        //Debug.Log($"Boss received {damage} damage!");
    }

    public void CheckIfDead()
    {
        if (_currentHealth <= 0)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        FindObjectOfType<Timer>().StopTiming();
        Time.timeScale = 0.5f;
        Destroy(this.gameObject);
        yield return new WaitForSecondsRealtime(2);
        SceneManager.LoadScene("Winning");
    }

    private void UpdatePhase1()
    {
        if (isActing)
        {
            return; // if acting, finish the action before moving again
        }

        if (isCasting)
        {
            return; // If casting, finish casting before moving again
        }

        if (Time.time < nextActionTime)
        {
            return; // Time between actions
        }

        if (_basicAttackUsed < _basicAttackToUse)
        {

            StartCoroutine(BasicAttackSequence());
        }
        else
        {
            _animator.SetTrigger("CastTrigger");

            //Selects a random ability from the ability list (This is for phase 1)
            BossAbilities abilities = GetComponent<BossAbilities>();
            if (abilities != null)
            {
                Ability selectedAbility = abilities.GetRandomAbility();

                if (selectedAbility != null)
                {
                    StartCoroutine(abilities.ExecuteAbility(selectedAbility));
                }
            }

            _basicAttackToUse = Random.Range(1, 5);
            _basicAttackUsed = 0;
            isCasting = false;
        }
    }

    private void UpdatePhase2()
    {
        if (isActing)
        {
            return;
        }
        if (Time.time < nextActionTime)
        {
            return;
        }

        StartCoroutine(PatrolBeforeAbility());
    }

    private IEnumerator PatrolBeforeAbility() // Move between point A and B. Afterwards, execute an ability
    {
        isActing = true;

        Transform targetPoint = (_currentMovingTarget == _pointA) ? _pointB : _pointA;

        if(_spriteMovement != null) {_spriteMovement.StartMoving();}

        while (Vector3.Distance(transform.position, targetPoint.position) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, _movingSpeed * Time.deltaTime);
            yield return null;
        }

        if (_spriteMovement != null) {_spriteMovement.StopMoving();}

        _currentMovingTarget = targetPoint;

        if (reachedTarget >= 1)
        {
            transform.Rotate(new Vector3(0, 1, 0), 180); // rotate boss when reach target
        }

        reachedTarget++;

        _animator.SetTrigger("CastTrigger");

        BossAbilities abilities = GetComponent<BossAbilities>();
        if (abilities != null)
        {
            Ability selectedAbility = abilities.GetRandomAbility();

            if (selectedAbility != null)
            {
               yield return StartCoroutine(abilities.ExecuteAbility(selectedAbility));
            }
        }

        nextActionTime = Time.time + actionCooldown;
        isActing = false;
    }

    private IEnumerator BasicAttackSequence()
    {
        isActing = true;

        _animator.SetTrigger("AttackTrigger");

        while (_basicAttackUsed < _basicAttackToUse)
        {
            _animator.SetTrigger("AttackTrigger");

            BasicAttack();
            _basicAttackUsed++;

            yield return new WaitForSeconds(_basicAttackCooldown);
        }

        yield return new WaitForSeconds(0.1f);

        nextActionTime = Time.time + actionCooldown;
        isActing = false;
    }

    private void BasicAttack()
    {
        Instantiate(_basicAttackPrefab, _basicAttackFirePoint.position, Quaternion.identity);
    }

}