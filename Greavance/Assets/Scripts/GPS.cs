using UnityEngine;

public class GPS : MonoBehaviour
{
    [SerializeField] GameObject roomAt;
    [SerializeField] GameObject gps;

    void Update()
    {
        if (roomAt.activeInHierarchy)
        {
            gps.SetActive(true);
        }
        else
        {
            gps.SetActive(false);
        }
    }
}
