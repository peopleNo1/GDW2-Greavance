using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class FireballProjectile : MonoBehaviour
{
    [Header("Fireball Settings")]
    [SerializeField] private float _damage = 10.0f;
    [SerializeField] private float _waitTime = 1f;
    [SerializeField] private float _fallSpeed = 5f;
    [SerializeField] private float _destroyDelay = 2f;
    
    [Header("Animation")]
    [SerializeField] private Animator _animator;
    
    private Rigidbody2D _rb;
    private bool _isFalling = false;
    private bool _initialized = false;
    
    public void Initialize(float damage, float waitTime, float fallSpeed)
    {
        _damage = damage;
        _waitTime = waitTime;
        _fallSpeed = fallSpeed;
        _initialized = true;
        
        StartCoroutine(WaitThenFall());
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        
        if (_rb != null)
        {
            _rb.gravityScale = 0;
            _rb.linearVelocity = Vector2.zero;
        };

        if (!_initialized)
        {
            _damage = 10f;
            _waitTime = 1f;
            _fallSpeed = 5f;
            StartCoroutine(WaitThenFall());
        }
    }
    
    private IEnumerator WaitThenFall()
    {
        if (_animator != null)
        {
            _animator.SetTrigger("Wait");
        }
        
        yield return new WaitForSeconds(_waitTime);
        
        StartFalling();
    }
    
    private void StartFalling()
    {
        _isFalling = true;
        
        if (_animator != null)
        {
            _animator.SetTrigger("Fall");
        }
        
        if (_rb != null)
        {
            _rb.gravityScale = 1f;
        }
        
        Destroy(gameObject, _destroyDelay);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(_damage);
            }
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Platform") && _isFalling)
        {
            Destroy(gameObject);
        }
    }
}
