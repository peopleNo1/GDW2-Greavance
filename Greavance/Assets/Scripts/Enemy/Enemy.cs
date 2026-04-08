using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    protected PlayerController playerController;
    private Animator _animator;

    [Header("Base Enemy Stats")]
    public float _damageCooldown = 1.0f;
    public float _enemySpeed = 1.0f;
    public float _visionRange = 1.0f;
    public float _deathDuration = 1.0f;

    [Header("Border Controls")]
    private Vector2 startingPos;

    [Header("Movement")]
    public Transform _playerPos;
    [SerializeField] private float leftBoundary = -5f;
    [SerializeField] private float rightBoundary = 5f;
    [SerializeField] private bool switchSides = true; // True = right, False = left

    [Header("Death Settings")]
    [SerializeField] private GameObject _deathEffectPrefab;
    [SerializeField] private float _destroyDelay = 0.5f;
    private bool _isDying = false;

    [HideInInspector]
    public float _maxHealth = 150f;
    public float _currentHealth = 150f;
    bool _calculatedThisFrame = false;
    public Vector3 _dirToPlayer;
    private bool changeDirection = false;
    float time;

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        ResetHealth();
    }

    protected virtual void Update()
    {
        UpdateEnemy();
    }

    protected virtual void LateUpdate()
    {
        _calculatedThisFrame = false;
    }

    public virtual void UpdateEnemy()
    {
        KillIfDead();
    }

    public void ResetHealth()
    {
        _currentHealth = _maxHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        GameObject bloodParticle = Instantiate(_deathEffectPrefab, transform.position, Quaternion.identity);
        Destroy(bloodParticle, _destroyDelay);
    }

    public void HealDamage(float heal)
    {
        _currentHealth += heal;

        if (_currentHealth >= _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
    }

    public void KillIfDead()
    {
        if (CheckIfDead())
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        _isDying = true;

        _rb.bodyType = RigidbodyType2D.Static;

        Collider2D col = GetComponent<Collider2D>();
        if(col != null){col.enabled = false;}

        if (_animator != null)
        {
            _animator.SetTrigger("Dead");

            yield break;
        }
    }

    private void DeathAnimComplete()
    {
        if (_deathEffectPrefab != null)
        {
            Instantiate(_deathEffectPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject, _destroyDelay);
    }

    public bool CheckIfDead()
    {
        if (_currentHealth <= 0.0f)
        {
            return true;
        }
        return false;
    }

    public void SetCustomBoundaries(float left, float right)
    {
        leftBoundary = left;
        rightBoundary = right;
    }

    public void SeekPlayer(Vector3 dirToPlayer)
    {
        float targetX;
        bool willHit;

        if (_rb.bodyType == RigidbodyType2D.Static)
        {
            return;
        }
        
        if (CanSeePlayer(_visionRange))
        {
            targetX = dirToPlayer.x * _enemySpeed;

            willHit = WillHitBoundary(targetX);

            if (willHit)
            {
                _rb.linearVelocityX = 0;
                transform.rotation = Quaternion.Euler(0, targetX > 0 ? 0 : 180, 0);
            }

            else
            {
                _rb.linearVelocityX = targetX;
                transform.rotation = Quaternion.Euler(0, targetX > 0 ? 0 : 180, 0);
            }
        }
        else
        {
            float direction = switchSides ? 1f : -1f;
            
            targetX = direction * _enemySpeed;
            
            willHit = WillHitBoundary(targetX);

            if (willHit)
            {
                _rb.linearVelocityX = 0;
                switchSides = !switchSides;
                transform.rotation = Quaternion.Euler(0, targetX > 0 ? 180 : 0, 0);
            }
            else
            {
                _rb.linearVelocityX = targetX;
                transform.rotation = Quaternion.Euler(0, targetX > 0 ? 0 : 180, 0);
            }
        }
    }

    public bool WillHitBoundary(float targetX)
    {
        bool willHit;
        if (targetX > 0 && transform.position.x > rightBoundary)
        {
            willHit = true;
        }
        else if (targetX < 0 && transform.position.x < leftBoundary)
        {
            willHit = true;
        }
        else
        {
            willHit = false;
        }

        return willHit;
    }

    public Vector3 GetDirToPlayer()
    {
        Vector3 dirToPlayer;

        if (!_calculatedThisFrame)
        {
            dirToPlayer = _playerPos.transform.position - transform.position;
            _dirToPlayer = dirToPlayer;
            _calculatedThisFrame = true;
        }
        else
        {
            dirToPlayer = _dirToPlayer;
        }

        return dirToPlayer;
    }

    public float GetDistToPlayer()
    {
        return GetDirToPlayer().magnitude;
    }

    public bool CanSeePlayer(float _visionRange)
    {
        if (GetDistToPlayer() <= _visionRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RotateTowardsDir(Vector3 dir)
    {
        float angle = Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        switchSides = !switchSides;
    }

    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Boss")) return;
        
        if (collision.gameObject.CompareTag("Player") && Time.realtimeSinceStartup >= time)
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                float damage = GetDamageAmount();
                player.TakeDamage(damage);
                time = Time.realtimeSinceStartup + _damageCooldown;
            }
        }
    }

    protected virtual float GetDamageAmount()
    {
        return 10f;
    }

    protected virtual void TriggerAttackAnimation()
    {
        // Override in derived classes
    }

    public float GetLeftBoundary()
    {
        return leftBoundary;
    }

    public float GetRightBoundary()
    {
        return rightBoundary;
    }

    public void SetAnimator(Animator currentEnemy)
    {
        _animator = currentEnemy;
    }
}