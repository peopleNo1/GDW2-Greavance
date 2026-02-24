using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private GameObject levelAt;
    [SerializeField] private GameObject[] roomList;

    private GameObject[] doors;

    //replace by other code
    private Vector2 input;
    [SerializeField] private float moveSpeed = 5.0f;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        doors = GameObject.FindGameObjectsWithTag("Door");

        foreach (GameObject room in roomList)
        {
            if (room.name != levelAt.name)
            {
                room.SetActive(false);
            }
        }
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

            if (levelAt.name == LayerMask.LayerToName(door.layer) && tele.IsAtDoor())
            {
                tele.GetLevelAt().SetActive(false);
                levelAt = tele.GetDestination();
                levelAt.SetActive(true);

                transform.position = tele.GetEnterPosition().transform.position;
            }
        }
    }
}
