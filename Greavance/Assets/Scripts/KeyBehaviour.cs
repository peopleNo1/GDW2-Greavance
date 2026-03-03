using UnityEngine;

public class KeyBehaviour : MonoBehaviour
{
    private bool enter;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        enter = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        enter = false;
    }

    public bool Enter()
    {
        return enter;
    }

    public void Collected()
    {
        Destroy(gameObject);
    }
}
