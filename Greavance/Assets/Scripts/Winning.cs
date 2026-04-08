using UnityEngine;
using UnityEngine.UI;

public class Winning : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] InputField input;
    private int time;
    private string playerName;

    void Start()
    {
        time = PlayerPrefs.GetInt("time", 0);
        text.text = "Your time: " + time;
    }

    public void GetPlayerName()
    {
        playerName = input.text;
    }

    public void SavePlayerName()
    {
        Debug.Log("time: " + time);
        if (playerName != null)
        {
            FindObjectOfType<Leaderboard>().SetRecord(playerName, time);
            Debug.Log(playerName);
        }
        else
        {
            FindObjectOfType<Leaderboard>().SetRecord(time);
        }
    }
}
