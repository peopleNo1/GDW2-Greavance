using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackController : MonoBehaviour
{
    public Transform _player;
    public float _speed = 2.0f;
    private float _distance;

    public float _moveForwardDistance = 2.0f;
    private float _moveBackDistance = 0.0f;

    public bool attack = false;
    private bool attackForward = false;
    private bool attackBack = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && attack == false)
        {
            _distance = 0f;
            attackForward = true;
            attackBack = false;
            attack = true;
        }

        if (attack)
        {
            AttackStarted();
        }


    }

    void AttackStarted()
    {
        //Moving forward
        if (attackForward)
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);

            _distance += Time.deltaTime;

            if (_distance >= _moveForwardDistance)
            {
                attackBack = true;
                attackForward = false;
            }
        }

        //Moving back
        if (attackBack)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);

            _distance -= Time.deltaTime;

            //Checks when player comes back to playyer
            if (_distance <= _moveBackDistance)
            {
                transform.position = _player.position;
                _distance = 0f;
                attackBack = false;
                attackForward = false;
                attack = false;
            }
        }
        
    }
}


        