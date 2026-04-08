using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    //private Rigidbody2D rb;
    [SerializeField] private GameObject levelAt;
    [SerializeField] private GameObject[] roomList;

    private GameObject[] doors;
    private GameObject[] keys;

    [SerializeField] Animated guideAni;

    [SerializeField] private bool isBoss;

    ////replace by other code
    //private Vector2 input;
    //[SerializeField] private float moveSpeed = 12.0f;

    void Start()
    {   if (!isBoss)
        {
            //rb = GetComponent<Rigidbody2D>();
            doors = GameObject.FindGameObjectsWithTag("Door");
            keys = GameObject.FindGameObjectsWithTag("Key");

            foreach (GameObject room in roomList)
            {
                if (room.name != levelAt.name)
                {
                    room.SetActive(false);
                }
            }
        }
    }

    void Update()
    {
        ////normal movement
        //input.x = Input.GetAxisRaw("Horizontal");
        //input.y = Input.GetAxisRaw("Vertical");

        //input.Normalize();

        // climb stairs
        if (!isBoss)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                CheckDoors();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                CheckKeys();
            }
        }
        //test
        if (Input.GetKeyDown(KeyCode.F))
        {
            guideAni.Flip();
        }
    }

    //private void FixedUpdate()
    //{
    //    rb.linearVelocity = input * moveSpeed;
    //}

    private void CheckDoors()
    {
        foreach (GameObject door in doors)
        {
            TeleportDoor tele = door.GetComponent<TeleportDoor>();

            if (tele.IsAtDoor() && Unlocked(tele.GetKeys()))
            {
                if (tele.gameObject.name == "DoorToBoss")
                {
                    SceneManager.LoadScene("BossBattle1");
                    return;
                }
                tele.GetLevelAt().SetActive(false);
                levelAt = tele.GetDestination();
                levelAt.SetActive(true);

                transform.position = tele.GetEnterPosition().transform.position;
                break;
            }
        }
    }

    private bool Unlocked(GameObject[] keysForDoor)
    {
        foreach(GameObject key in keysForDoor)
        {
            if (key != null)
            {
                return false;
            }
        }
        return true;
    }

    private void CheckKeys()
    {
        foreach (GameObject key in keys)
        {
            if (key != null)
            {
                KeyBehaviour qed = key.GetComponent<KeyBehaviour>();
                if (qed.Enter())
                {
                    qed.Collected();
                    break;
                }
            }
        }
    }
}
