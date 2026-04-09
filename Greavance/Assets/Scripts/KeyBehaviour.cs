using UnityEngine;
using UnityEngine.UI;

public class KeyBehaviour : MonoBehaviour
{
    [SerializeField] GameObject image;

    private bool enter;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Head")
        {
            enter = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Head")
        {
            enter = false;
        }
    }

    public bool Enter()
    {
        return enter;
    }

    public void Collected()
    {
        image.GetComponent<Image>().color = Color.white;
        Destroy(gameObject);
    }
}
