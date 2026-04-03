using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RangedEnemy : Enemy
{
    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("Stats")]
    [SerializeField] public float _desiredMaxHp = 50.0f; //Desired max hp overrides the default hp from enemy script
    [SerializeField] public float _damage = 10.0f;
    [SerializeField] private float _arrowCooldown = 2.0f;
    [SerializeField] private GameObject _arrowPrefab;
    [SerializeField] private Transform _firePoint;

    [HideInInspector]
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

        _timeSinceLastArrow += Time.deltaTime;

        animator = GetComponent<Animator>();
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

    public override void TakeDamage(float damage)
    {
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