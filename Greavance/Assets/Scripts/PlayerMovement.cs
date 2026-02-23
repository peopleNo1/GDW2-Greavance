using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private GameObject levelAt;

    private GameObject[] doors;

    private Vector2 input;
    [SerializeField] private float moveSpeed = 5.0f;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        doors = GameObject.FindGameObjectsWithTag("door");
    }

    void Update()
    {
        //normal movement
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        
        input.Normalize();

        //climb stairs
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckDoors();
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = input * moveSpeed;
    }

    private void CheckDoors()
    {
        foreach (GameObject door in doors)
        {
            TeleportDoor tele = door.GetComponent<TeleportDoor>();
            if (tele.IsAtDoor())
            {
                transform.position = tele.getDestination().transform.position;
                levelAt = tele.getLevelAt();
            }
        }
    }
}
