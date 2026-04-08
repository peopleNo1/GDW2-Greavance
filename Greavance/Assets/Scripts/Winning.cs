using UnityEngine;
using UnityEngine.UI;

public class Winning : MonoBehaviour
{
    [SerializeField] Text text;

    void Start()
    {
        text.text = "Your time: " + PlayerPrefs.GetInt("totalRecords", 0);
    }
}
