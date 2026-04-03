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

    //Keep track of which platform has an enemy on
    private List<Vector3> _spawnPositions = new List<Vector3>();
    private List<float> _leftBoundaries = new List<float>();
    private List<float> _rightBoundaries = new List<float>();
    private List<Transform> _platformTransforms = new List<Transform>();

    private Transform _player;
    private PhaseManager _phaseManager;
    private Boss boss;

    protected override void Awake()
    {
        base.Awake();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _phaseManager = GetComponent<PhaseManager>();
        boss = GetComponent<Boss>();

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

    public override bool CanUse()
    {
        if (_phaseManager.isPhase2 == false)
        {
            return false;
        }
        float healthPercent = boss._currentHealth / boss._maxHealth * 100f;
        return healthPercent <= 50f;
    }

    public override IEnumerator Execute()
    {
        int enemyCount = Random.Range(_minEnemies, _maxEnemies + 1);

        _spawnPositions.Clear();
        _leftBoundaries.Clear();
        _rightBoundaries.Clear();
        _platformTransforms.Clear();

        foreach (Transform point in _groundSpawnPoints)
        {
            if (point != null)
            {
                GetSpawnPointData(point, false);
            }
        }

        foreach (Transform point in _platformSpawnPoints)
        {
            if (point != null)
            {
                if (_platformOccupancy.ContainsKey(point) && _platformOccupancy[point])
                {
                    continue; // Skip occupied platforms
                }
                GetSpawnPointData(point, true);
            }
        }

        if (_spawnPositions.Count == 0)
        {
            yield break;
        }

        int actualSpawnCount = Mathf.Min(enemyCount, _spawnPositions.Count);

        for (int i = 0; i < actualSpawnCount; i++)
        {
            if (_enemyPrefabs == null || _enemyPrefabs.Length == 0)
            {
                Debug.LogError("No enemy prefabs assigned!");
                yield break;
            }
            
            GameObject enemyPrefab = _enemyPrefabs[Random.Range(0, _enemyPrefabs.Length)];

            int spawnIndex = Random.Range(0, _spawnPositions.Count);
            
            // Get spawn data from lists
            Vector3 spawnPos = _spawnPositions[spawnIndex];
            float leftBoundary = _leftBoundaries[spawnIndex];
            float rightBoundary = _rightBoundaries[spawnIndex];
            Transform platformTransform = _platformTransforms[spawnIndex];

            GameObject summonEffect = null;
            if (_summonCirclePrefab != null)
            {
                summonEffect = Instantiate(_summonCirclePrefab, spawnPos, Quaternion.identity);
            }

            yield return new WaitForSeconds(_animationDuration);

            if (summonEffect != null)
            {
                Destroy(summonEffect);
            }
            
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            
            Enemy enemyComponent = newEnemy.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                enemyComponent.SetCustomBoundaries(leftBoundary, rightBoundary);
                Debug.Log($"Spawned {enemyPrefab.name} with boundaries: {leftBoundary} to {rightBoundary}");
            }

            // Mark platform as occupied if it's a platform spawn
            if (platformTransform != null)
            {
                _platformOccupancy[platformTransform] = true;
                _spawnedEnemies[platformTransform] = newEnemy;
                StartCoroutine(MonitorEnemyDeath(newEnemy, platformTransform));
            }

            // Remove used spawn point
            _spawnPositions.RemoveAt(spawnIndex);
            _leftBoundaries.RemoveAt(spawnIndex);
            _rightBoundaries.RemoveAt(spawnIndex);
            _platformTransforms.RemoveAt(spawnIndex);
            
            if (i < enemyCount - 1)
            {
                yield return new WaitForSeconds(_spawnDelay);
            }
        }
    }

    private void GetSpawnPointData(Transform point, bool isPlatform)
    {
        Vector3 spawnPos = point.position;
        float leftBoundary = spawnPos.x - 2f;
        float rightBoundary = spawnPos.x + 2f;
        Transform platformTransform = isPlatform ? point : null;
        
        Collider2D[] colliders = Physics2D.OverlapPointAll(point.position);
        
        foreach (Collider2D col in colliders)
        {
            if ((isPlatform && col.gameObject.layer == LayerMask.NameToLayer("Platform")) || 
                (!isPlatform && col.gameObject.layer == LayerMask.NameToLayer("Ground")))
            {
                Bounds bounds = col.bounds;
                leftBoundary = bounds.min.x + 0.3f;
                rightBoundary = bounds.max.x - 0.3f;
                spawnPos = new Vector3(point.position.x, bounds.max.y + 0.3f, 0);
                break;
            }
        }
        
        _spawnPositions.Add(spawnPos);
        _leftBoundaries.Add(leftBoundary);
        _rightBoundaries.Add(rightBoundary);
        _platformTransforms.Add(platformTransform);
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

    private void OnDestroy()
    {
        _platformOccupancy.Clear();
        _spawnedEnemies.Clear();
    }
}