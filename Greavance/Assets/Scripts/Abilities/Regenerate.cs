using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Regenerate : Ability
{
    [Header("Regenerate Settings")]
    [SerializeField] private float _healAmount = 40f;
    [SerializeField] private float _castTime = 2f;
    [SerializeField] private GameObject _castEffectPrefab;
    [SerializeField] private GameObject _shieldPrefab;

    [Header("Shield Settings")]
    [SerializeField] private int _shieldHealth = 50;
    [SerializeField] private float _shieldDestroyDelay = 0.5f;

    protected Boss boss;
    private bool _hasRegenerated = false;
    private bool _isCasting = false;
    private GameObject _activeShield;
    private GameObject _activeCastEffect;
    private Coroutine _castCoroutine;

    protected override void Awake()
    {
        base.Awake();

        boss = GetComponent<Boss>();
    }

    public override bool CanUse()
    {
        if (_hasRegenerated)
        {
            return false;
        }
        float healthPercent = (boss._currentHealth / boss._maxHealth) * 100f;
        return healthPercent <= 50f;
    }

    public override IEnumerator Execute()
    {
        if (_hasRegenerated || _isCasting) yield break;

        _isCasting = true;

        if (_shieldPrefab != null)
        {
            _activeShield = Instantiate(_shieldPrefab, transform.position, Quaternion.identity);
            _activeShield.transform.parent = transform;

            Shield shieldComponent = _activeShield.AddComponent<Shield>();
            shieldComponent.Initialize(_shieldHealth, this);
        }

        if (_castEffectPrefab != null)
        {
            _activeCastEffect = Instantiate(_castEffectPrefab, transform.position, Quaternion.identity);
            _activeCastEffect.transform.parent = transform;
        }

        float timer = 0f;
        while (timer < _castTime && _isCasting)
        {
            //If shield is destroyed, _isCasting will be set to false
            if (!_isCasting)
            {
                break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        if (_isCasting && timer >= _castTime)
        {
            CompleteRegeneration();
        }
        else
        {
            InterruptRegeneration();
        }
    }

    private void CompleteRegeneration()
    {
        if (boss != null)
        {
            boss._currentHealth += _healAmount;
            if (boss._currentHealth > boss._maxHealth)
            {
                boss._currentHealth = boss._maxHealth;
            }
        }

        _hasRegenerated = true;
        CleanUp();
    }

    private void InterruptRegeneration()
    {
        CleanUp();
    }

    private void CleanUp()
    {
        _isCasting = false;

        if (_activeShield != null)
        {
            Destroy(_activeShield);
            _activeShield = null;
        }

        if (_activeCastEffect != null)
        {
            Destroy(_activeCastEffect);
            _activeCastEffect = null;
        }
    }

    public void OnShieldDestroy()
    {
        if (_isCasting)
        {
            _isCasting = false;
        }
    }
}