using UnityEngine;
using UnityEngine.UI;

public class UILayer : MonoBehaviour
{
    public void OrderToTop()
    {
        transform.SetSiblingIndex(transform.childCount - 1);
    }
}
