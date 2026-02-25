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

    Vector3 _dirToPlayer;
    static GameObject _player;
    public Transform _playerPos;
    bool _calculatedThisFrame = false;
    public float _enemySpeed = 5.0f;
    public float _enemyDamage = 0.5f;
    public float _visionRange = 10.0f;

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerPos = GameObject.FindGameObjectWithTag("Player").transform;
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
        _rb.linearVelocity = dirToPlayer.normalized * _enemySpeed;
    }

    public void FleePlayer(Vector3 dirToPlayer)
    {
        _rb.linearVelocity = dirToPlayer.normalized * -_enemySpeed;
    }

    public Vector3 GetDirToPlayer()
    {
        Vector3 dirToPlayer;

        if (!_calculatedThisFrame)
        {
            dirToPlayer = _player.transform.position - transform.position;
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
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            // _player.TakeDamage(_contactDamage);
        }
    }
}