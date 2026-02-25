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

    [Header("Boss Phase 2")]
    [SerializeField] private float _phase2HP = 50f;//Health percentage that the boss needs to reach in order to change phase
    [SerializeField] public int numMobs = 2;
    
    private void Awake()
    {
        boss = GetComponent<Boss>();
        bossAbilities = GetComponent<BossAbilities>();


        neededHP = boss._maxHealth * _phase2HP / 100f;
    }

    private void Update()
    {
        CheckIfPhase2();
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

        bossAbilities.abilityHistory.Clear();

        boss.nextActionTime = Time.time;
    }
}