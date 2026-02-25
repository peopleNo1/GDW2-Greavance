using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossAbilities : MonoBehaviour
{
    protected Boss boss;
    protected Ability abilities;

    [SerializeField] private Fireball _fireballAbility;
    [SerializeField] private Explosion _explosionAbility;
    [SerializeField] private BossSummon _summonAbility;
    [SerializeField] private Regenerate _regenerateAbility;

    public Queue<string> abilityHistory = new Queue<string>();
    private List<Ability> _availableAbilities;

    private void Awake()
    {
        boss = GetComponent<Boss>();
        abilities = GetComponent<Ability>();

        _availableAbilities = new List<Ability>();

        if (_fireballAbility != null) _availableAbilities.Add(_fireballAbility);
        if (_explosionAbility != null) _availableAbilities.Add(_explosionAbility);
        if (_summonAbility != null) _availableAbilities.Add(_summonAbility);
        if (_regenerateAbility != null) _availableAbilities.Add(_regenerateAbility);
    }

    public Ability GetRandomAbility()
    {
        List<Ability> validAbilities = new List<Ability>();

        foreach (Ability ability in _availableAbilities)
        {
            if(!ability.CanUse()) continue;

            if (!abilityHistory.Contains(ability.name))
            {
                validAbilities.Add(ability);
            }
        }

        if (validAbilities.Count == 0)
        {
            foreach (Ability ability in _availableAbilities)
            {
                if (ability.CanUse())
                {
                    validAbilities.Add(ability);
                }
            }
        }

        if (validAbilities.Count > 0)
        {
            return validAbilities[Random.Range(0, validAbilities.Count)];
        }

        return null;
    }

    public IEnumerator ExecuteAbility(Ability ability)
    {
        boss.isCasting = true;

        boss.TriggerCasting();

        abilityHistory.Enqueue(ability.name);
        if (abilityHistory.Count > 2)
        {
            abilityHistory.Dequeue();
        }

        Debug.Log($"Boss using ability: {ability.name}");

        yield return ability.Execute();

        boss.nextActionTime = Time.time + boss.actionCooldown;
        boss.isCasting = false;

        yield return new WaitForSeconds(0.1f);
    }
}