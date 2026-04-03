using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Fireball : Ability
{
    [SerializeField] private GameObject _fireballPrefab;

    [Header("Fireball Settings")]
    [SerializeField] private float _fireballHeight = 4f;
    [SerializeField] private float _damage = 10.0f;

    private Transform _player;

    protected override void Awake()
    {
        base.Awake();

        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override IEnumerator Execute()
    {
        Debug.Log("Boss: Fireball!");

        Vector3 _spawnPos = _player.position + Vector3.up * _fireballHeight;
        
        Instantiate(_fireballPrefab, _spawnPos, Quaternion.identity);

        yield return new WaitForSeconds(0.3f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            playerController.TakeDamage(_damage);
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.CompareTag("Boss") || collision.gameObject.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), GetComponent<Collider2D>());
            return;
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            return;
        }
    }
}