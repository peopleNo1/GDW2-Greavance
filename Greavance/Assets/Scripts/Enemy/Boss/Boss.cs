using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Boss : Enemy
{
    protected PhaseManager phaseManager;
    protected BossBasicAttacks bossBasicAttacks;

    public bool isActing = false;
    public bool isCasting = false;
    public float actionCooldown = 5f;
    public float nextActionTime = 0f;

    [Header("Animation")]
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Basic Attack Settings")]
    [SerializeField] private int _basicAttackUsed = 0;
    private int _basicAttackToUse = 0;
    [SerializeField] private GameObject _basicAttackPrefab;
    [SerializeField] private float _basicAttackCooldown = 0.5f;
    [SerializeField] private Transform _basicAttackFirePoint;

    //Patrol settings
    [Header("Patrol Settings")]
    private float _patrolTimer;
    [SerializeField] private float _patrolDuration = 2f;
    private Transform _currentMovingTarget;
    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;
    [SerializeField] private float _movingSpeed = 2f;

    protected override void Awake()
    {
        base.Awake();

        phaseManager = GetComponent<PhaseManager>();
        bossBasicAttacks = GetComponent<BossBasicAttacks>();
    }

    public override void UpdateEnemy()
    {
        base.UpdateEnemy();

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
        if (_pointA != null)
        {
            _currentMovingTarget = _pointA;
        }
        nextActionTime = Time.time;

        ResetHealth();
    }

    private void UpdatePhase1()
    {
        // _animator.SetBool("isActing", isActing);
        // _animator.SetBool("isCasting", isCasting);

        if (isActing)
        {
            return;
        }

        if (isCasting)
        {
            return;
        }

        if (Time.time < nextActionTime)
        {
            return;
        }

        if (_basicAttackUsed < _basicAttackToUse)
        {
            StartCoroutine(BasicAttackSequence());
        }
        else
        {
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

    private IEnumerator PatrolBeforeAbility()
    {
        isActing = true;

        _patrolTimer = 0f;

        Transform _target = (_currentMovingTarget == _pointA) ? _pointB : _pointA;

        while (_patrolTimer < _patrolDuration)
        {
            if (_pointA && _pointB != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, _target.position, _movingSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, _target.position) < 0.1f)
                {
                    _currentMovingTarget = _target;
                    _target = (_target == _pointA) ? _pointB : _pointA;
                }
            }

            _patrolTimer += Time.deltaTime;
            yield return null;
        }

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
        Instantiate(_basicAttackPrefab, _basicAttackFirePoint.position, _basicAttackFirePoint.rotation);
        Debug.Log("Basic Attack Used!");
    }

    public void TriggerCasting()
    {
        _animator.SetTrigger("CastTrigger");
    }
}