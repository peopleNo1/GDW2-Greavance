using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmallEnemy : Enemy
{
    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("Stats")]
    [SerializeField] public float _desiredMaxHp = 50.0f;
    [SerializeField] public float _damage = 10.0f;

    protected override void Awake()
    {
        base.Awake();
        _maxHealth = _desiredMaxHp;
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
        animator.SetTrigger("WasDamaged");

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
