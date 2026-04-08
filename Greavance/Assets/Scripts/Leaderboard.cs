using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    public int totalRecords;
    private string setName = "player";

    public void ResetBoard()
    {
        totalRecords = 0;
        PlayerPrefs.SetInt("totalRecords", 0);
        for (int i = 0; i < 10; i++)
        {
            PlayerPrefs.SetString(setName + i.ToString(), "Unnamed user");
            PlayerPrefs.SetInt(setName + i.ToString(), 0);
        }
        PlayerPrefs.Save();
        Debug.Log("record reset");
    }

    public void SetRecord(int player, int record)
    {
        totalRecords = PlayerPrefs.GetInt("totalRecords", 0) + 1;
        PlayerPrefs.SetInt("totalRecords", totalRecords);
        PlayerPrefs.SetString(setName + player.ToString(), "Unnamed user");
        PlayerPrefs.SetInt(setName + player.ToString(), 0);
        PlayerPrefs.Save();
    }

    public void SetRecord(int player, string playerName, int record)
    {
        totalRecords = PlayerPrefs.GetInt("totalRecords", 0) + 1;
        PlayerPrefs.SetInt("totalRecords", totalRecords);
        PlayerPrefs.SetString(setName + player.ToString(), playerName);
        PlayerPrefs.SetInt(setName + player.ToString(), 0);
        PlayerPrefs.Save();
    }
}
