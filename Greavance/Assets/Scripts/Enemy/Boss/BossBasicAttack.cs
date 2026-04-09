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
            if (_animator != null)
            {
                _animator.SetTrigger("Collide");
                _hasCollided = true;

                _speed = 0;

                float animLength = GetCurrentAnimationLength();
                Destroy(gameObject, animLength > 0 ? animLength : _destroyDelayAfterCollide);
            }

            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            playerController.TakeDamage(_damage);
            

            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Boss") || collision.gameObject.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(collision, GetComponent<Collider2D>());
            return;
        }
    }

    private float GetCurrentAnimationLength()
    {
        if (_animator == null) return 0;
        
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.length;
    }
}