using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] public float maxHP = 100;
    [SerializeField] public float currentHP;

    [SerializeField] private CameraFollow cameraPlayer;
    [SerializeField] private ArmController arm;
    public Transform armSpawnPoint;
    public bool PlayerTurn = true;

    float horizontalInput;
    public float _moveSpeed = 5f;

    public float _jumpForce = 7f;
    bool _isJumping = false;

    bool _facingRight = false;
    private Vector2 _moveInput;
    
    Rigidbody2D _rb;
    Animator _ani;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _ani = GetComponent<Animator>();

        ResetHealth();
    }

    // Update is called once per frame
    void Update()
    {
        KillIfDead();

        if (PlayerTurn)
        {
            //Horizontal Input system
            horizontalInput = Input.GetAxis("Horizontal");
            
            FlipSprite();

            if (Input.GetButtonDown("Jump") && !_isJumping)
            {
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _jumpForce);
                _isJumping = true;
            }

            
            
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            //Camera changes to arm
            cameraPlayer.ChangeTarget();
        }
    }

    private void FixedUpdate()
    {
        if (PlayerTurn)
        {
            //Moves player when Input for horizontal movement is pressed
            _rb.linearVelocity = new Vector2(horizontalInput * _moveSpeed, _rb.linearVelocity.y);

            _ani.SetFloat("xVelocity", Math.Abs(_rb.linearVelocity.x));
        }
    }

    //Flips the player if sprite is in wrong direction
    void FlipSprite()
    {
        if(_facingRight && horizontalInput < 0f || !_facingRight && horizontalInput > 0f)
        {
            _facingRight = !_facingRight;
            Vector3 _dir = transform.localScale;
            _dir.x *= -1f;
            transform.localScale = _dir;
        }
    }

    public void KillIfDead()
    {
        if (CheckIfDead())
        {
            Destroy(gameObject);
        }
    }

    public bool CheckIfDead()
    {
        if (currentHP <= 0.0f)
        {
            return true;
        }
        return false;
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;

        Debug.Log($"Current HP: {currentHP}");
    }

    public void ResetHealth()
    {
        currentHP = maxHP;
    }

    //Checks if player is touching the ground.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        _isJumping = false;
    }
}
