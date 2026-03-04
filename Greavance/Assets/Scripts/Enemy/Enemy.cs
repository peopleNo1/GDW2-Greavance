using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    Rigidbody2D _rb;

    [Header("Base Enemy Stats")]
    public float _maxHealth = 50.0f;

    public float _currentHealth;

    public float _contactDamage = 10.0f;
    public float _damageCooldown = 1.0f;

    public float _lastDamageTime;
    private PlayerController playerController;

    Vector3 _dirToPlayer;
    public GameObject _player;
    public Transform _playerPos;
    bool _calculatedThisFrame = false;
    public float _enemySpeed = 1.0f;
    public float _enemyDamage = 0.5f;
    public float _visionRange = 1.0f;

    [Header("Border Controls")]
    private Vector2 startingPos;

    [Header("Movement")]
    [SerializeField] private float leftBoundary = -5f;
    [SerializeField] private float rightBoundary = 5f;
    [SerializeField] private bool switchSides = true; // True = right, False = left

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        _lastDamageTime = Time.time;
    }

    private void Start()
    {
        ResetHealth();
    }

    private void Update()
    {
        UpdateEnemy();
    }

    private void LateUpdate()
    {
        _calculatedThisFrame = false;
    }

    public virtual void UpdateEnemy()
    {
        KillIfDead();
        _dirToPlayer = GetDirToPlayer();
        SeekPlayer(_dirToPlayer);

    }

    public void ResetHealth()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
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
            Destroy(gameObject);
        }
    }

    public bool CheckIfDead()
    {
        if (_currentHealth <= 0.0f)
        {
            return true;
        }
        return false;
    }

    public void SeekPlayer(Vector3 dirToPlayer)
    {
        float targetX;
        bool willHit;

        if (CanSeePlayer(_visionRange))
        {
            targetX = dirToPlayer.x * _enemySpeed;

            willHit = WillHitBoundary(targetX);

            if (willHit)
            {
                _rb.linearVelocityX = 0;
            }

            else
            {
                _rb.linearVelocityX = targetX;
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
                Debug.Log($"Enemy reversing direction at {transform.position.x}");
            }
            else
            {
                _rb.linearVelocityX = targetX;
            }
        }
    }

    public bool WillHitBoundary(float targetX)
    {
        bool willHit;
        if (targetX > 0 && transform.position.x >= rightBoundary)
        {
            willHit = true;
        }
        else if (targetX < 0 && transform.position.x <= leftBoundary)
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

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (Time.time >= _lastDamageTime + _damageCooldown)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (playerController != null)
                {
                    Debug.Log($"Damage delt to player: {_contactDamage}");
                    playerController.TakeDamage(_contactDamage);
                    _lastDamageTime = Time.time;
                }
            }
        }
    }
}