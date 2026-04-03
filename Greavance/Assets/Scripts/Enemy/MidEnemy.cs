using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MidEnemy : Enemy
{
    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("Stats")]
    [SerializeField] public float _desiredMaxHp = 150.0f;
    [SerializeField] public float _desiredCooldown = 0.5f;
    [SerializeField] private float _damage = 15.0f;

    [HideInInspector]
    public bool wasDamaged;

    protected override void Awake()
    {
        base.Awake();
        _maxHealth = _desiredMaxHp;
        _damageCooldown = _desiredCooldown;
        animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

    public override void UpdateEnemy()
    {
        KillIfDead();
        _dirToPlayer = GetDirToPlayer();
        SeekPlayer(_dirToPlayer);
    }

    public override void TakeDamage(float damage)
    {
        Debug.Log($"MidEnemy TakeDamage called on {gameObject.name}");
        if (animator == null)
        {
            Debug.LogError($"Animator is NULL on MidEnemy: {gameObject.name}");
            animator = GetComponent<Animator>();

            if (animator == null)
            {
                Debug.LogError($"Still couldn't find Animator on {gameObject.name}");
            }
        }

        if (animator != null)
        {
            Debug.Log($"Setting WasDamaged trigger on {gameObject.name}");
            animator.SetTrigger("WasDamaged");
        }
        else
        {
            Debug.LogError($"Cannot play animation - animator is null on {gameObject.name}");
        }

        base.TakeDamage(damage);
    }

    protected override float GetDamageAmount()
    {
        return _damage;
    }

    protected override void TriggerAttackAnimation()
    {
        if (animator != null)
            animator.SetTrigger("Attack");
    }
}
