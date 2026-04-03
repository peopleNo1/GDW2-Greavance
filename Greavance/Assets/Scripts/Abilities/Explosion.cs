using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Explosion : Ability
{
    [Header("Explosion Prefabs")]
    [SerializeField] private GameObject _warningPrefab;
    [SerializeField] private GameObject _explosionPrefab;

    [Header("Explosion Settings")]
    [SerializeField] private float _warningDuration = 0.1f;
    [SerializeField] private float _delayBetweenExplosions = 0.2f;
    [SerializeField] private int _groundWarningCount = 10;
    [SerializeField] private int _platformWarningCount = 5;
    [SerializeField] private float _damage = 20f;
    [SerializeField] private float _explosionLifeTime = 1f;

    [Header("Detection")]
    [SerializeField] private float _platformCheckDistance = 1.5f;


    private Transform _player;
    private static LayerMask _groundLayer;
    private static LayerMask _platformLayer;


    private List<GameObject> _activeWarnings = new List<GameObject>();
    private List<Vector2> _explosionPositions = new List<Vector2>();

    static Explosion()
    {
        _groundLayer = 1 << 10;
        _platformLayer = 1 << 12;
    }

    protected override void Awake()
    {
        base.Awake();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            _player = playerObj.transform;
        }
    }

    private void Start()
    {
        if (_player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                _player = playerObj.transform;
            }
        }
    }

    public override IEnumerator Execute()
    {
        if (_player == null)
        {      
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                _player = playerObj.transform;
            }
            else
            {
                yield break;
            }
        }

        bool playerOnPlatform = IsPlayerOnPlatform();
        string targetType = playerOnPlatform ? "Platforms" : "Ground";
        Debug.Log($"Explosion targeting {targetType}");

        int warningCount = playerOnPlatform ? _platformWarningCount : _groundWarningCount;

        GameObject[] targetObjects = FindTargetObjects(playerOnPlatform);

        if (targetObjects.Length == 0)
        {
            Debug.LogWarning($"No {(playerOnPlatform ? "platforms" : "ground")} found");
            yield break;
        }

        _activeWarnings.Clear();
        _explosionPositions.Clear();

        foreach(GameObject target in targetObjects)
        {
            yield return StartCoroutine(CreateWarningTrailOnTarget(target, warningCount));
        }

        yield return new WaitForSeconds(_warningDuration);

        bool hasDamagedPlayer = false;

        for (int i = 0; i < _explosionPositions.Count; i++)
        {
            if (_explosionPrefab != null)
            {
                GameObject explosion = Instantiate(_explosionPrefab, _explosionPositions[i], Quaternion.identity);

                StartCoroutine(DestroyExplosionAfterTime(explosion, _explosionLifeTime));
            }

            if (!hasDamagedPlayer && _player != null && Vector2.Distance(_player.position, _explosionPositions[i]) <= 2f)
            {
                PlayerController playerController = _player.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.TakeDamage(_damage);
                    hasDamagedPlayer = true;
                }
                Debug.Log("Player hit by explosion");
            }

            if (i < _activeWarnings.Count && _activeWarnings[i] != null)
            {
                Destroy(_activeWarnings[i]);
            }

            if (i < _explosionPositions.Count - 1)
            {
                yield return new WaitForSeconds(_delayBetweenExplosions);
            }
        }

        foreach (GameObject warning in _activeWarnings)
        {
            if(warning != null){Destroy(warning);}
        }
        _activeWarnings.Clear();
        _explosionPositions.Clear();
    }

    private IEnumerator DestroyExplosionAfterTime(GameObject explosion, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (explosion != null)
        {
            Destroy(explosion);
        }
    }

    private IEnumerator CreateWarningTrailOnTarget(GameObject target, int _numberOfWarnings)
    {
        Collider2D targetCollider = target.GetComponent<Collider2D>();
        if(targetCollider == null){yield break;}

        Bounds bounds = targetCollider.bounds;

        float playerX = _player.position.x;
        float startX, endX;

        if (playerX < bounds.center.x)
        {
            startX = bounds.max.x;
            endX = bounds.min.x;
            Debug.Log($"Trail on target: Right -> Left");
        }
        else
        {
            startX = bounds.min.x;
            endX = bounds.max.x;
            Debug.Log($"Trail on target: Left -> Right");
        }

        float yOffset = 1f;

        for (int i = 0; i < _numberOfWarnings; i++)
        {
            float t = (float)i / (_numberOfWarnings - 1);
            float xPos = Mathf.Lerp(startX, endX, t);

            Vector2 warningPos = new Vector2(xPos, bounds.center.y + yOffset);

            GameObject warning = Instantiate(_warningPrefab, warningPos, Quaternion.identity);

            warning.SetActive(true);

            warning.transform.parent = target.transform;

            _activeWarnings.Add(warning);
            _explosionPositions.Add(warningPos);

            yield return new WaitForSeconds(0.05f);
        }
    }

    private bool IsPlayerOnPlatform()
    {
        RaycastHit2D hit = Physics2D.Raycast(_player.position, Vector2.down, _platformCheckDistance, _platformLayer);
        if (hit.collider != null)
        {
            Debug.Log($"Player on: {hit.collider.gameObject.name} (Layer: {LayerMask.LayerToName(hit.collider.gameObject.layer)})");
        }
        return hit.collider != null;
    }

    private GameObject[] FindTargetObjects(bool findPlatforms)
    {
        Debug.DrawRay(_player.position, Vector2.down * _platformCheckDistance, Color.red, 2f);

        LayerMask targetLayer = findPlatforms ? _platformLayer : _groundLayer;

        Collider2D[] colliders = Physics2D.OverlapBoxAll(Vector2.zero, new Vector2(1000, 1000), 0, targetLayer);

        HashSet<GameObject> uniqueObjects = new HashSet<GameObject>();

        foreach (Collider2D collider in colliders)
        {
            uniqueObjects.Add(collider.gameObject);
        }

        GameObject[] result = new GameObject[uniqueObjects.Count];
        uniqueObjects.CopyTo(result);

        return result;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Boss"))
        {
            Boss boss = collision.gameObject.GetComponent<Boss>();
            if (boss != null && gameObject.name.Contains("Explosion"))
            {
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.GetComponent<Collider2D>());
                return;
            }
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            playerController.TakeDamage(_damage);
        }
        else if (collision.gameObject.CompareTag("Boss") || collision.gameObject.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.GetComponent<Collider2D>());
            return;
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Platform") || collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            return;
        }
    }
}