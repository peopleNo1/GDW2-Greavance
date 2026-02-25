using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Fireball : Ability
{
    [SerializeField] private GameObject _fireballPrefab;

    [Header("Fireball Settings")]
    [SerializeField] private float _fireballHeight = 4f;

    private Transform _player;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override IEnumerator Execute()
    {
        Debug.Log("Fireball!");

        Debug.Log("Boss: Fireball!");

        Vector3 _spawnPos = _player.position + Vector3.up * _fireballHeight;
        
        Instantiate(_fireballPrefab, _spawnPos, Quaternion.identity);

        yield return new WaitForSeconds(0.3f);
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