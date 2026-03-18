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


    Rigidbody2D _rb;

    [SerializeField] float maxSpeed = 5;


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
        }
        //Check if it can teleport to home
        else if (HeadTurn == false)
        {
            _rb.position = _pos[1].transform.position;
        }
    }

}

