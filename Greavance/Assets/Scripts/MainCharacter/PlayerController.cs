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

    private GamePlayControl UIControl;

    [SerializeField] private CameraFollow cameraPlayer;
    [SerializeField] private AttackController attackCon;
    public Transform armSpawnPoint;
    public bool PlayerTurn = true;

    float horizontalInput;
    public float _moveSpeed = 5f;
    

    public float _jumpForce = 7f;
    bool _isGrounded = false;

    bool _facingRight = false;
    private Vector2 _moveInput;
    
    Rigidbody2D _rb;
    Animator _ani;

    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _ani = GetComponent<Animator>();
        UIControl = GetComponent<GamePlayControl>();

        ResetHealth();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //Camera changes to arm
            cameraPlayer.ChangeTarget();
        }

        if (PlayerTurn)
        {
            //Horizontal Input system
            horizontalInput = Input.GetAxis("Horizontal");

            FlipSprite();

            if (Input.GetButtonDown("Jump") && _isGrounded)
            {
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _jumpForce);
                _isGrounded = false;
                _ani.SetBool("isJumping", !_isGrounded);
            }

            
        }


    }

    private void FixedUpdate()
    {
        if (PlayerTurn)
        {
            //Moves player when Input for horizontal movement is pressed
            _rb.linearVelocity = new Vector2(horizontalInput * _moveSpeed, _rb.linearVelocity.y);
            _ani.SetFloat("xVelocity", Math.Abs(_rb.linearVelocity.x));
            _ani.SetFloat("yVelocity", _rb.linearVelocity.y);
            _ani.SetBool("attacking", attackCon.attack);
            
            
        }
    }

    //Flips the player if sprite is in wrong direction
    private void FlipSprite()
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
        UIControl.setHealth(currentHP);

        Debug.Log($"Current HP: {currentHP}");
    }

    public void ResetHealth()
    {
        currentHP = maxHP;
    }

    //Checks if player is touching the ground.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _isGrounded = true;
        _ani.SetBool("isJumping", !_isGrounded);
    }

   
}
