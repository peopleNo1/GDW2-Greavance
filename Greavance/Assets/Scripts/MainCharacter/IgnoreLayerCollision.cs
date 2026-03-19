using UnityEngine;

public class IgnoreLayerCollision : MonoBehaviour
{

    void Start()
    {
        Physics.IgnoreLayerCollision(12, 13);
    }


}
