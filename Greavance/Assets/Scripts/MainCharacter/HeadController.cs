using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;

public class HeadController : MonoBehaviour
{
    [SerializeField] private CameraFollow cameraArm;
    public List<Transform> _pos;

    public bool ArmTurn = false;

    //Movement Input
    float horizontalInput;
    public float _moveSpeed = 5f;
    //bool _facingRight = false;
    private Vector2 _moveInput;

    Rigidbody2D _rb;


    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        _rb.position = _pos[1].transform.position;
    }


    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); // A/D keys, Left/Right arrow keys
        
        Vector2 MoveDirection = new Vector2(horizontalInput * _moveSpeed, 0f);

        _rb.AddForce(MoveDirection);

    }



    /*
    

    // Update is called once per frame
    void Update()
    {
        
         //Horizontal Input system
         horizontalInput = Input.GetAxis("Horizontal");

         FlipSprite();

         
        
    }

    private void FixedUpdate()
    {
        if (ArmTurn)
        {
            //Moves player when Input for horizontal movement is pressed
            _rb.linearVelocity = new Vector2(horizontalInput * _moveSpeed, _rb.linearVelocity.y);

            _ani.SetFloat("xVelocity", Math.Abs(_rb.linearVelocity.x));
        }

    }

    
    //Flips the arm if sprite is in wrong direction
    void FlipSprite()
    {
        if (_facingRight && horizontalInput < 0f || !_facingRight && horizontalInput > 0f)
        {
            _facingRight = !_facingRight;
            Vector3 _dir = transform.localScale;
            _dir.x *= -1f;
            transform.localScale = _dir;
        }
    }
    */

    public void TeleportArm(bool ArmTurn)
    {
        //Check if it can teleport to player
        if (ArmTurn == true)
        {
            _rb.position = _pos[0].transform.position;
        }
        //Check if it can teleport to home
        else if (ArmTurn == false)
        {
            _rb.position = _pos[1].transform.position;
        }
    }
}

