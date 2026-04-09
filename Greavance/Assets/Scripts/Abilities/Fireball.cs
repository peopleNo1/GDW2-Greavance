using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Fireball : Ability
{
    [SerializeField] private GameObject _fireballPrefab;

    [Header("Fireball Settings")]
    [SerializeField] private float _fireballHeight = 4f;
    [SerializeField] private float _damage = 10.0f;

    [Header("Wait and Fall Settings")]
    [SerializeField] private float _waitTime = 1f;
    [SerializeField] private float _fallSpeed = 5f;

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

        GameObject fireball = Instantiate(_fireballPrefab, _spawnPos, Quaternion.identity);
        
        FireballProjectile projectile = fireball.GetComponent<FireballProjectile>();

        if (projectile != null)
        {
            projectile.Initialize(_damage, _waitTime, _fallSpeed);
        }

        yield return new WaitForSeconds(0.3f);
    }

}