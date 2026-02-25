using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhaseManager : MonoBehaviour
{
    //Composition calls
    protected Boss boss;
    protected BossAbilities bossAbilities;

    [SerializeField] public bool isPhase2 = false;
    public float neededHP;

    [Header("Debug")]
    [SerializeField] private bool _startInPhase2 = false;

    [Header("Boss Phase 2")]
    [SerializeField] private GameObject _phase2Effect;
    [SerializeField] private float _phase2HP = 50f;//Health percentage that the boss needs to reach in order to change phase
    [SerializeField] public int numMobs = 2;

    [Header("Phase 2 Platforms")]
    [SerializeField] private GameObject _platformPrefab;
    [SerializeField] private Transform[] _platformSpawnPoints;
    private List<GameObject> _spawnedPlatforms = new List<GameObject>();
    
    private void Awake()
    {
        boss = GetComponent<Boss>();
        bossAbilities = GetComponent<BossAbilities>();


        neededHP = boss._maxHealth * _phase2HP / 100f;
    }

    private void Start()
    {
        if (_startInPhase2)
        {
            ForcePhase2();
        }
    }

    private void Update()
    {
        if (!_startInPhase2 && !isPhase2)
        {
            CheckIfPhase2();
        }
    }

    private void ForcePhase2()
    {
        boss._currentHealth = neededHP;

        isPhase2 = true;
        InitiatePhase2();
    }

    public void CheckIfPhase2()
    {
        if (isPhase2 == false && boss._currentHealth <= boss._maxHealth * _phase2HP / 100f)
        {
            InitiatePhase2();
        }
    }

    private void InitiatePhase2()
    {
        isPhase2 = true;

        if (_phase2Effect != null)
        {
            Instantiate(_phase2Effect, transform.position, Quaternion.identity);
        }

        SpawnPhase2Platforms();

        bossAbilities.abilityHistory.Clear();

        boss.nextActionTime = Time.time;

        Debug.Log("Phase 2 Platforms spawned");
    }

    private void SpawnPhase2Platforms()
    {
        foreach (Transform spawnPoint in _platformSpawnPoints)
        {
            if (spawnPoint != null)
            {
                GameObject newPlatform = Instantiate(_platformPrefab, spawnPoint.position, spawnPoint.rotation);
                _spawnedPlatforms.Add(newPlatform);

                newPlatform.transform.parent = transform.parent;
            }
        }

        Debug.Log($"Spawned {_spawnedPlatforms.Count} platforms for Phase 2");
    }
}