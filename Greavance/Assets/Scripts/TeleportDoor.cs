using UnityEngine;

public class TeleportDoor : MonoBehaviour
{
    [SerializeField] private GameObject levelAt;
    [SerializeField] private GameObject destination;

    private bool isAtDoor = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isAtDoor = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isAtDoor = false;
    }

    public bool IsAtDoor()
    {
        return isAtDoor;
    }

    public GameObject getLevelAt()
    {
        return levelAt;
    }

    public GameObject getDestination()
    {
        return destination;
    }
}