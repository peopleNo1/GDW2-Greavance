using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Regenerate : Ability
{
    [SerializeField] private float _healAmount = 40f;
    [SerializeField] private float _castTime = 2f;
    [SerializeField] private GameObject _castEffectPrefab;

    protected Boss boss;
    private bool _hasRegenerated = false;

    private void Awake()
    {
        boss = GetComponent<Boss>();
    }

    public override bool CanUse()
    {
        return !_hasRegenerated;
    }

    public override IEnumerator Execute()
    {
        if (_hasRegenerated) yield break;

        Debug.Log("Casting Regenerate...");

        if (_castEffectPrefab != null)
        {
            GameObject effect = Instantiate(_castEffectPrefab, transform.position + Vector3.up * 2f, Quaternion.identity);
            Destroy(effect, _castTime);
        }

        yield return new WaitForSeconds(_castTime);

        boss._currentHealth += _healAmount;
        if (boss._currentHealth > boss._maxHealth)
        {
            boss._currentHealth = boss._maxHealth;
        }
        _hasRegenerated = true;
        Debug.Log($"Regenerated! Health: {boss._currentHealth}");
    }
}