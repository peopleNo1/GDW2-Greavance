using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossSpriteMovement : MonoBehaviour
{
    [Header("Sprite Movement Settings")]
    [SerializeField] private Sprite[] _startMovingFrames;
    [SerializeField] private Sprite[] _loopFrames;
    [SerializeField] private  Sprite[] _stopMovingFrames;

    private Boss _boss;
    private bool _isMoving = false;

    private void Awake()
    {
        _boss = GetComponent<Boss>();
    }

    public void StartMoving()
    {
        _boss._animator.SetBool("isMoving", true);
    }

    public void StopMoving()
    {
        _boss._animator.SetBool("isMoving", false);
    }

    public void SetFacingDirection(bool facingRight)
    {
        Vector3 scale = transform.localScale;
        scale.x = facingRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }
}
