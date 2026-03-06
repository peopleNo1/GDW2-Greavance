using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AttackAngle : MonoBehaviour
{
    public Transform _player;
    public GameObject _ArmAngle;
    public float _damage = 5;

    public void Update()
    {
        transform.position = _player.position;
        Vector2 aimPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        AimPlayer(aimPos);
    }
    private void AimPlayer(Vector2 aimPos)
    {
        //Look at dir
        Vector2 lookAtDir;

        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(_ArmAngle.transform.position);
        lookAtDir = aimPos - new Vector2(playerScreenPoint.x, playerScreenPoint.y);
        lookAtDir.Normalize();

        float angle = Mathf.Atan2(-lookAtDir.x, lookAtDir.y) * Mathf.Rad2Deg;

        //Quaternion angle axis around forward
        _ArmAngle.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
