using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossBasicAttacks : MonoBehaviour
{
    [SerializeField] public float _damage = 10.0f;
    [SerializeField] public float _speed = 3f;
    private Vector2 _direction = Vector2.down;

    private void Update()
    {
        transform.Translate(_direction * _speed * Time.deltaTime);
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //_player = GameObject.FindGameObjectWithTag("Player");
            //_player.TakeDamage(_contactDamage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Boss") || collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
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