using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Explosion : Ability
{
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private float _explosionRadius = 4f;
    [SerializeField] private float _explosionDelay = 1f;
    [SerializeField] private int _damage = 20;
    private List<GameObject> _platforms;

    private Transform _player;
    private LayerMask _groundLayer;

    private float _platformRadius = 1.5f;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _groundLayer = LayerMask.NameToLayer("Ground");
    }

    public override IEnumerator Execute()
    {
        if (!IsPlayerOnPlatform())
        {
            Vector3 _targetPos = _player.position;

            GameObject warning = Instantiate(_explosionPrefab, _targetPos, Quaternion.identity);

            Debug.Log("Ground Explosion!");

            yield return new WaitForSeconds(_explosionDelay);

            if (Vector3.Distance(_player.position, _targetPos) <= _explosionRadius)
            {
                //Implement damage to player later
                Debug.Log($"Player hit for {_damage} damage!");
            }

            Destroy(warning);
        }
        else if (IsPlayerOnPlatform())
        {
            foreach (GameObject platform in _platforms)
            {
                Vector3 _platformPos = platform.transform.position;

                GameObject warning = Instantiate(_explosionPrefab, _platformPos, Quaternion.identity);

                Debug.Log("Ground Explosion!");

                yield return new WaitForSeconds(_explosionDelay);

                if (Vector3.Distance(_player.position, _platformPos) <= _explosionRadius)
                {
                    //Implement damage to player later
                    Debug.Log($"Player hit for {_damage} damage!");
                }

                Destroy(warning);
            }
        }
    }

    private bool IsPlayerOnPlatform()
    {
        RaycastHit2D hit = Physics2D.Raycast(_player.position, Vector2.down, _platformRadius, _groundLayer);

        return hit.collider != null;
    }
}