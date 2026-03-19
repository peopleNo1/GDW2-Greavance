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

    public bool wasDamaged;

    protected override void Awake()
    {
        base.Awake();
        _maxHealth = _desiredMaxHp;
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
                    playerController.TakeDamage(_damage);
                    _lastDamageTime = Time.time;
                }
            }
        }
    }
}
