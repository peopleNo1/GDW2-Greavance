using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameObject levelAt;
    private List<TeleportDoor> doors;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //normal movement
        if (Input.GetKeyDown(KeyCode.A))
        {

        }
        else if (Input.GetKeyDown(KeyCode.D))
        {

        }

        //climb stairs
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckDoor();
        }
    }

    private bool CheckDoor()
    {
        foreach (TeleportDoor door in doors)
        {
            if (door.IsAtDoor())
            {
                return true;
            }
        }
        return false;
    }
}
