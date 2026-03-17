using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public List<Transform> targetList = new List<Transform>();  // Targets for camera
    [SerializeField] private PlayerController Player;
    [SerializeField] private HeadController Head;

    public float distance = 5f;  // distance behind the player
    public float height = 3f; // height above the player
    private Transform target;

    private void Start()
    {
        target = targetList[0];
    }

    // LateUpdate() ensures camera moves after player movement
    void LateUpdate()
    {
        // calculate camera position based on player's position, forward, and height
        
            Vector3 pos = target.position
                        - target.forward * distance
                        + Vector3.up * height;

            // Move the camera to that position
            transform.position = pos;

            // Make the camera look at the player
            transform.LookAt(target);
        
       
    }

    public void ChangeTarget()
    {
        //Check if its player turn to switch to arm
        if (target == targetList[0])
        {
            Player.PlayerTurn = false;
            Head.ArmTurn = true;
            Head.TeleportArm(true);

            target = targetList[1];
        }
        //Check if its arm turn to switch to player
        else if (target == targetList[1])
        {
            Player.PlayerTurn = true;
            Head.ArmTurn = false;

            Head.TeleportArm(false);

            target = targetList[0];
        }
    }
}