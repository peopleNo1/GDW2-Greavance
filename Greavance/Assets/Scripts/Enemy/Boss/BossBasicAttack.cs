using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossBasicAttacks : MonoBehaviour
{
    [SerializeField] public float _damage = 10.0f;
    [SerializeField] public float _speed = 3f;
    PlayerController _player;
    private Vector2 _direction = Vector2.down;

    private void Update()
    {
        transform.Translate(_direction * _speed * Time.deltaTime);
        _player = GetComponent<PlayerController>();
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boss")) {return;}
            
        if (collision.gameObject.CompareTag("Player"))
        {
            _player.TakeDamage(_damage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Boss") || 
                collision.gameObject.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
            return;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}