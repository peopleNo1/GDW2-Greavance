using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CameraFollow cameraPlayer;
    [SerializeField] private AttackController attackCon;
    public Transform armSpawnPoint;
    public bool PlayerTurn = true;

    float horizontalInput;
    public float _moveSpeed = 5f;

    public ParticleSystem AttackParticle;
    public ParticleSystem DamageParticle;

    public float _jumpForce = 7f;
    //bool _jumped = false;
    bool _isGrounded = false;
    bool _walking = true;
   // private Vector2 _moveInput;

    bool _facingRight = true;
    private Vector2 _moveInput;

    public bool _dead = false;

    //public bool _doubleJump = false;
    

    Rigidbody2D _rb;
    Animator _ani;

    [SerializeField] float maxHealth = 100;
    [SerializeField] float currentHealth;
    private GamePlayControl gamePlayControl;

    bool start = true;
    public bool isbossfight;
    public GameObject playerIcon;
    public Sprite deadImage;

    void Awake()
    {
        Vector3 _dir = transform.localScale;
        _dir.x *= -1f;
        transform.localScale = _dir;
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _ani = GetComponent<Animator>();
        gamePlayControl = GetComponent<GamePlayControl>();

        DamageParticle.Stop();
        AttackParticle.Stop();

        ResetHealth();

        start = false;
        FindObjectOfType<Timer>().SetDone(true);
        FindObjectOfType<AudioManager>().Play("MainSong");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isbossfight)
        {
            if (start && Input.anyKeyDown)
            {
                start = false;
                FindObjectOfType<Timer>().SetDone(true);
            }
        }

        //Checks if player is dead
        if (!_dead)
        {

            //Makes head appear when pressing Q and player is grounded
            if (Input.GetKeyDown(KeyCode.Q) && _isGrounded && !FindObjectOfType<AttackController>().attack)
            {
                //Camera changes to arm
                cameraPlayer.ChangeTarget();
                //If its not players turn, player plays no head animation
                if (!PlayerTurn)
                {
                    _ani.SetBool("withHead", false);
                   
                }
                else
                {
                    _ani.SetBool("withHead", true);
                    
                }
            }



            //Hotkey for player dead  FIX THIS PART TO CONNECT WITH HEALTH
            //now jusst test is working
            if (Input.GetKeyDown(KeyCode.K) && _isGrounded)
            {
                _dead = true;
            }

            if (PlayerTurn)
            {
                //Horizontal Input system
                horizontalInput = Input.GetAxis("Horizontal");

                
                FlipSprite();

                /*
                if (Input.GetButtonDown("Jump") && !_isGrounded && _doubleJump)
                if (Input.GetButtonDown("Jump") && !_isGrounded)
                {
                    _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _jumpForce);
                    // _doubleJump = false;
                    Debug.Log("DoubleJump");
                }
                */


                if (Input.GetButtonDown("Jump") && _isGrounded)
                {
                    _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _jumpForce);
                    _isGrounded = false;
                    //_doubleJump = true;
                    FindObjectOfType<AudioManager>().Play("Jump");
                    _ani.SetBool("isJumping", !_isGrounded);
                }

                //Makes sure the sound of the footsteps dont repeat infinetly withouth finishing the sound effect
                if (horizontalInput != 0 && _isGrounded && _walking)
                {
                    StartCoroutine(WalkingSound());
                    _walking = false;
                }
                else if (_rb.linearVelocity.x == 0 && _isGrounded && !_walking)
                {
                    FindObjectOfType<AudioManager>().Stop("Walk");
                }
            }
            else
            {
                _rb.linearVelocityX = 0;
            }
        }
        else
        {
            Time.timeScale = 0;
            _ani.updateMode = AnimatorUpdateMode.UnscaledTime;
            _ani.SetBool("Dead", true);
            playerIcon.GetComponent<Image>().sprite = deadImage;
            FindObjectOfType<AudioManager>().Stop("MainSong");
            StartCoroutine(Die());
        }
    }

    public IEnumerator Die()
    {
        FindObjectOfType<AudioManager>().Play("Death");
        yield return new WaitForSecondsRealtime(2.5f);
        SceneManager.LoadScene("Die");
    }

    public IEnumerator WalkingSound()
    {
        FindObjectOfType<AudioManager>().Play("Walk");
        yield return new WaitForSeconds(0.45f);
        FindObjectOfType<AudioManager>().Stop("Walk");
        _walking = true;
    }

    private void FixedUpdate()
    {
        if (PlayerTurn && !_dead)
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
        if (_facingRight && horizontalInput < 0f || !_facingRight && horizontalInput > 0f)
        {
            _facingRight = !_facingRight;
            Vector3 _dir = transform.localScale;
            _dir.x *= -1f;
            transform.localScale = _dir;
        }
    }

    //Checks if player is touching the ground.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _isGrounded = true;
        
        _ani.SetBool("isJumping", !_isGrounded);
    }

    public void  ResetHealth()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        DamageParticle.Play();
        FindObjectOfType<AudioManager>().Play("Hurt");
        if (currentHealth <= 0f)
        {
            gamePlayControl.setHealth(0);
            _dead = true;
        }
        else
        {
            gamePlayControl.setHealth(currentHealth);
        }
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}
