using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossBasicAttacks : MonoBehaviour
{
    [SerializeField] public float _damage = 10.0f;
    [SerializeField] public float _speed = 3f;
    [SerializeField] public Animator _animator;
    private float _destroyDelayAfterCollide = 0.3f;
    private bool _hasCollided = false;
    private bool _hasDealtDamage = false;

    private Vector2 _direction = Vector2.left;

    private void Start()
    {
        _animator.SetTrigger("Launch");

        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        transform.Translate(_direction * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(_hasCollided){return;}

        if (collision.gameObject.CompareTag("Player"))
        {
            _hasCollided = true;

            _speed = 0;
            
            _animator.SetTrigger("Collide");

            if (!_hasDealtDamage)
            {
                PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.TakeDamage(_damage);
                    _hasDealtDamage = true;
                }
            }

            float animLength = GetCurrentAnimationLength();
            float destroyDelay = animLength > 0 ? animLength : _destroyDelayAfterCollide;

            Destroy(gameObject, destroyDelay);
        }
        else if (collision.gameObject.CompareTag("Boss") || collision.gameObject.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(collision, GetComponent<Collider2D>());
            return;
        }
        else if (collision.gameObject.CompareTag("Explosion"))
        {
            Physics2D.IgnoreCollision(collision, GetComponent<Collider2D>());
            return;
        }
        else
        {
            Destroy(gameObject, _destroyDelayAfterCollide);
        }
    }

    private float GetCurrentAnimationLength()
    {
        if (_animator == null) return 0;
        
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.length;
    }
}