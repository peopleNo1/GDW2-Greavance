using UnityEngine;

public class IgnoreLayerCollision : MonoBehaviour
{
   
    void Start()
    {
        Physics.IgnoreLayerCollision(6, 7);
    }

    
}
