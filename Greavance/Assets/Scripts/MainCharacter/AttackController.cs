using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackController : MonoBehaviour
{
    public PlayerController _player;
    public Transform _playerTrans;
    public float _speed = 2.0f;
    private float _distance;

    public float _moveForwardDistance = 2.0f;
    private float _moveBackDistance = 0.0f;

    public bool attack = false;
    private bool attackForward = false;
    private bool attackBack = false;

    Renderer myRenderer;
    public HeadController head;

    [SerializeField] float attackDamage = 30;

    private void Awake()
    {
        myRenderer = GetComponent<Renderer>();
        myRenderer.enabled = false;
    }

    void Update()
    {
        //Checks if; player clicks button, is not attacking, its head's turn, and players isnt dead
        if (Input.GetKeyDown(KeyCode.Mouse0) && attack == false && head.HeadTurn == false && _player._dead == false)
        {
            _distance = 0f;
            attackForward = true;
            attackBack = false;
            attack = true;
            myRenderer.enabled = true;
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
                transform.position = _playerTrans.position;
                _distance = 0f;
                attackBack = false;
                attackForward = false;
                attack = false;
                myRenderer.enabled = false;
            }
        }

    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if(!attack){return;}

    //    if (collision.gameObject.tag == "Enemy")
    //    {
    //        Debug.Log("Player damaged enemy");
    //        collision.gameObject.GetComponent<Enemy>().TakeDamage(attackDamage);
    //    }
    //    if (collision.gameObject.tag == "Boss")
    //    {
    //        Debug.Log("Player damaged boss");
    //        Boss boss = collision.gameObject.GetComponent<Boss>();
    //        boss.TakeDamage(attackDamage);
    //    }
    //}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!attack) { return; }

        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Player damaged enemy");
            collision.gameObject.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
        if (collision.gameObject.tag == "Boss")
        {
            Debug.Log("Player damaged boss");
            Boss boss = collision.gameObject.GetComponent<Boss>();
            boss.TakeDamage(attackDamage);
        }
    }
}