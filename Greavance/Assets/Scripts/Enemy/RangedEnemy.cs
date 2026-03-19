using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RangedEnemy : Enemy
{
    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("Stats")]
    [SerializeField] public float _desiredMaxHp = 50.0f;
    [SerializeField] public float _damage = 10.0f;
    [SerializeField] private float _arrowCooldown = 2.0f;
    [SerializeField] private GameObject _arrowPrefab;
    [SerializeField] private Transform _firePoint;

    [HideInInspector]
    public bool wasDamaged;
    private float _timeSinceLastArrow = 0.0f;

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

        _timeSinceLastArrow += Time.deltaTime;
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

    public override void UpdateEnemy()
    {
        KillIfDead();

        if (_timeSinceLastArrow >= _arrowCooldown)
        {
            ShootArrow();
        }
    }

    public void CheckIFDamaged()
    {
        if (wasDamaged)
        {
            animator.SetTrigger("wasDamaged");
        }
    }

    private void ShootArrow()
    {
        animator.SetTrigger("Attack");

        if (_arrowPrefab != null && _firePoint != null)
        {
            Instantiate(_arrowPrefab, _firePoint.position, _firePoint.rotation);
        }
        else if (_arrowPrefab == null)
        {
            Debug.LogWarning("Arrow prefab is null");
        }
        else
        {
            Debug.LogWarning("Fire point is null");
        }

        _timeSinceLastArrow = 0f;
    }
}