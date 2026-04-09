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
        text.text = "It tooks you " + time + " seconds";
    }

    public void GetPlayerName()
    {
        playerName = input.text;
    }

    public void SavePlayerName()
    {
        if (playerName != null)
        {
            FindObjectOfType<Leaderboard>().SetRecord(playerName, time);
        }
        else
        {
            FindObjectOfType<Leaderboard>().SetRecord(time);
        }
    }
}
