using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;

public class HeadController : MonoBehaviour
{
    [SerializeField] private CameraFollow cameraArm;
    public List<Transform> _pos;

    public bool HeadTurn = false;

    //Movement Input
    float horizontalInput;
    public float _moveSpeed = 5f;
    //bool _facingRight = false;
    private Vector2 _moveInput;


    bool _rollingRight = true;
    bool _rollingLeft = true;
    public float _rollingSoundRange = 2;
   
    Rigidbody2D _rb;

   [SerializeField] float maxSpeed = 5;


    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        _rb.position = _pos[1].transform.position;
    }


    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal"); // A/D keys, Left/Right arrow keys

        Vector2 MoveDirection = new Vector2(horizontalInput * _moveSpeed, 0f);

        _rb.AddForce(MoveDirection);

        //Makes sure the sound of the rolling right dont repeat infinetly withouth finishing the sound effect
        if (_rb.linearVelocity.x > _rollingSoundRange && _rollingRight)
        {
            StartCoroutine(RollingRightSound());
            _rollingRight = false;
        }
        //Makes sure the sound of the rolling left dont repeat infinetly withouth finishing the sound effect
        else if (_rb.linearVelocity.x < _rollingSoundRange && _rollingLeft)
        {
            StartCoroutine(RollingLeftSound());
            _rollingLeft = false;
        }
        else if (_rb.linearVelocity.x == _rollingSoundRange && !_rollingLeft && !_rollingRight)
        {
            FindObjectOfType<AudioManager>().Stop("RollingRight");
            FindObjectOfType<AudioManager>().Stop("RollingLeft");
        }
    }

    private void FixedUpdate()
    {
        if (_rb.linearVelocity.magnitude > maxSpeed)
        {
            _rb.linearVelocity = Vector3.ClampMagnitude(_rb.linearVelocity, maxSpeed);
        }
    }

    public void TeleportHead(bool HeadTurn)
    {
        //Check if it can teleport to player
        if (HeadTurn == true)
        {
            _rb.position = _pos[0].transform.position;
            FindObjectOfType<AudioManager>().Play("DropHead");
        }
        //Check if it can teleport to home
        else if (HeadTurn == false)
        {
            _rb.position = _pos[1].transform.position;
        }
    }

    public IEnumerator RollingRightSound()
    {
        FindObjectOfType<AudioManager>().Play("RollingRight");
        yield return new WaitForSeconds(1.1f);
        FindObjectOfType<AudioManager>().Stop("RollingRight");
        _rollingRight = true;
    }
    
    public IEnumerator RollingLeftSound()
    {
        FindObjectOfType<AudioManager>().Play("RollingLeft");
        yield return new WaitForSeconds(1.2f);
        FindObjectOfType<AudioManager>().Stop("RollingLeft");
        _rollingLeft = true;
    }
}

