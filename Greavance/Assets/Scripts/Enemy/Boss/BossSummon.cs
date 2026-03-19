using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossSummon : Ability
{
    [Header("Summon Settings")]
    [SerializeField] private GameObject[] _enemyPrefabs;
    [SerializeField] private int _minEnemies = 2;
    [SerializeField] private int _maxEnemies = 5;
    [SerializeField] private float _spawnDelay = 0.3f;

    [Header("Animation")]
    [SerializeField] private GameObject _summonCirclePrefab;
    [SerializeField] private float _animationDuration = 2.0f;

    [Header("Spawn Locations")]
    [SerializeField] private Transform[] _groundSpawnPoints;
    [SerializeField] private Transform[] _platformSpawnPoints;
    [SerializeField] private bool _allowRandomPlacement = true;

    private Dictionary<Transform, bool> _platformOccupancy = new Dictionary<Transform, bool>();
    private Dictionary<Transform, GameObject> _spawnedEnemies = new Dictionary<Transform, GameObject>();

    private Transform _player;
    private PhaseManager _phaseManager;

    protected override void Awake()
    {
        base.Awake();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _phaseManager = GetComponent<PhaseManager>();

        if (_platformSpawnPoints != null)
        {
            foreach (Transform point in _platformSpawnPoints)
            {
                if (point != null && !_platformOccupancy.ContainsKey(point))
                {
                    _platformOccupancy[point] = false;
                }
            }
        }
    }

    public override IEnumerator Execute()
    {
        int enemyCount = Random.Range(_minEnemies, _maxEnemies + 1);

        List<SpawnPointInfo> groundSpawns = new List<SpawnPointInfo>();
        List<SpawnPointInfo> platformSpawns = new List<SpawnPointInfo>();

        foreach (Transform point in _groundSpawnPoints)
        {
            if (point != null)
            {
                SpawnPointInfo info = GetSpawnPointInfo(point, false);
                if (info != null)
                {
                    groundSpawns.Add(info);
                }
            }
        }

        foreach (Transform point in _platformSpawnPoints)
        {
            if (point != null)
            {
                if (_platformOccupancy.ContainsKey(point) && _platformOccupancy[point])
                {
                    continue;
                }
                SpawnPointInfo info = GetSpawnPointInfo(point, true);
                if (info != null)
                {
                    info.platformTransform = point;
                    platformSpawns.Add(info);
                }
            }
        }

        List<SpawnPointInfo> availableSpawns = new List<SpawnPointInfo>();
        availableSpawns.AddRange(groundSpawns);
        availableSpawns.AddRange(platformSpawns);

        if (availableSpawns.Count == 0)
        {
            yield break;
        }

        int actualSpawnCount = Mathf.Min(enemyCount, availableSpawns.Count);

        for (int i = 0; i < actualSpawnCount; i++)
        {
            if (_enemyPrefabs == null || _enemyPrefabs.Length == 0)
            {
                Debug.LogError("No enemy prefabs assigned!");
                yield break;
            }
            
            GameObject enemyPrefab = _enemyPrefabs[Random.Range(0, _enemyPrefabs.Length)];

            int spawnIndex = Random.Range(0, availableSpawns.Count);
            SpawnPointInfo spawnInfo = availableSpawns[spawnIndex];

            GameObject summonEffect = null;
            if (_summonCirclePrefab != null)
            {
                summonEffect = Instantiate(_summonCirclePrefab, spawnInfo.position, Quaternion.identity);
            }

            yield return new WaitForSeconds(_animationDuration);

            if (summonEffect != null)
            {
                Destroy(summonEffect);
            }
            
            GameObject newEnemy = Instantiate(enemyPrefab, spawnInfo.position, Quaternion.identity);
            
            Enemy enemyComponent = newEnemy.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                enemyComponent.SetCustomBoundaries(spawnInfo.leftBoundary, spawnInfo.rightBoundary);
                Debug.Log($"Spawned {enemyPrefab.name} with boundaries: {spawnInfo.leftBoundary} to {spawnInfo.rightBoundary}");
            }

            if (spawnInfo.platformTransform != null)
            {
                _platformOccupancy[spawnInfo.platformTransform] = true;
                _spawnedEnemies[spawnInfo.platformTransform] = newEnemy;

                StartCoroutine(MonitorEnemyDeath(newEnemy, spawnInfo.platformTransform));
            }

            availableSpawns.RemoveAt(spawnIndex);
            
            if (i < enemyCount - 1)
            {
                yield return new WaitForSeconds(_spawnDelay);
            }
        }
    }

    private IEnumerator MonitorEnemyDeath(GameObject enemy, Transform platform)
    {
        while (enemy != null)
        {
            yield return new WaitForSeconds(0.5f);
        }
        
        if (_platformOccupancy.ContainsKey(platform))
        {
            _platformOccupancy[platform] = false;
        }
        
        if (_spawnedEnemies.ContainsKey(platform))
        {
            _spawnedEnemies.Remove(platform);
        }
    }

    private SpawnPointInfo GetSpawnPointInfo(Transform point, bool isPlatform)
    {
        SpawnPointInfo info = new SpawnPointInfo();
        info.position = point.position;
        
        Collider2D[] colliders = Physics2D.OverlapPointAll(point.position);
        
        foreach (Collider2D col in colliders)
        {
            if ((isPlatform && col.gameObject.layer == LayerMask.NameToLayer("Platform")) || (!isPlatform && col.gameObject.layer == LayerMask.NameToLayer("Ground")))
            {
                Bounds bounds = col.bounds;
                info.leftBoundary = bounds.min.x + 0.3f; 
                info.rightBoundary = bounds.max.x - 0.3f;
                info.position = new Vector3(point.position.x, bounds.max.y + 0.3f, 0);

                if (isPlatform)
                {
                    info.platformTransform = point;
                }

                return info;
            }
        }
        
        info.leftBoundary = point.position.x - 2f;
        info.rightBoundary = point.position.x + 2f;
        
        return info;
    }

    private void OnDestroy()
    {
        _platformOccupancy.Clear();
        _spawnedEnemies.Clear();
    }

    [System.Serializable]
    private class SpawnPointInfo
    {
        public Vector3 position;
        public float leftBoundary;
        public float rightBoundary;
        public Transform platformTransform;
    }
}