using UnityEngine;

public class TeleportDoor : MonoBehaviour
{
    [SerializeField] private GameObject levelAt;
    [SerializeField] private GameObject destination;
    private bool isAtDoor = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isAtDoor = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        isAtDoor= false;
    }

    public bool IsAtDoor()
    {
        return isAtDoor;
    }
}