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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
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
}