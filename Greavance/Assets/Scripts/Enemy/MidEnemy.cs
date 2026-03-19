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
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        CheckIFDamaged();
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

    public void CheckIFDamaged()
    {
        if (wasDamaged)
        {
            animator.SetTrigger("wasDamaged");
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boss")) {return;}
            
        if (Time.time >= _lastDamageTime + _damageCooldown)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (playerController != null)
                {
                    Debug.Log($"Damage delt to player: {_damage}");
                    animator.SetTrigger("Attack");
                    playerController.TakeDamage(_damage);
                    _lastDamageTime = Time.time;
                }
            }
        }
    }
}
