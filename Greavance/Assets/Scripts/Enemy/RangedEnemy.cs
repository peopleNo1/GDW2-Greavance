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
    private bool _isShooting = false;

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

        animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();

        _timeSinceLastArrow += Time.deltaTime;
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

    public override void UpdateEnemy()
    {
        KillIfDead();

        if (!_isShooting && _timeSinceLastArrow >= _arrowCooldown)
        {
            StartCoroutine(ShootArrow());
        }
    }

    public override void TakeDamage(float damage)
    {
        animator.SetTrigger("WasDamaged");

        base.TakeDamage(damage);
    }

    private IEnumerator ShootArrow()
    {
        _isShooting = true;

        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(GetAnimationLength("Attack"));

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
        _isShooting = false;
    }

    private float GetAnimationLength(string animationName)
    {
        if (animator == null) return 0.5f;
        
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name.ToLower().Contains(animationName.ToLower()))
            {
                return clip.length;
            }
        }
        return 0.5f;
    }
}