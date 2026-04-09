using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] Text[] playerNameDisplay;
    [SerializeField] Text[] timeDisplay;
    [SerializeField] bool displaying;

    public int[] records = new int[10];
    public int totalRecords;
    private string setName = "player";

    void Start()
    {
        if (displaying)
            UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        for (int i = 0; i < records.Length; i++)
        {
            playerNameDisplay[i].text = PlayerPrefs.GetString(setName + "str" + i.ToString(), "¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X");
            if (PlayerPrefs.GetInt(setName + i.ToString(), 0) == 0)
            {
                timeDisplay[i].text = "0000";
            }
            else 
            {
                timeDisplay[i].text = PlayerPrefs.GetInt(setName + i.ToString(), 0).ToString();
            }
        }
    }

    public void ResetBoard()
    {
        totalRecords = 0;
        PlayerPrefs.SetInt("totalRecords", 0);
        for (int i = 0; i < 10; i++)
        {
            PlayerPrefs.SetString(setName + "str" + i.ToString(), "¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X");
            PlayerPrefs.SetInt(setName + i.ToString(), 0);
            if (displaying)
            {
                playerNameDisplay[i].text = "¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X¡X";
                timeDisplay[i].text = "0000";
            }
        }
        PlayerPrefs.Save();
        Debug.Log("record reset");
    }

    public void SetRecord(int record)
    {
        if (totalRecords < 10)
        {
            totalRecords = PlayerPrefs.GetInt("totalRecords", 0) + 1;
            PlayerPrefs.SetInt("totalRecords", totalRecords);
        }
        int i = CompareRecord(record);
        if (i != 10)
        {
            PlayerPrefs.SetString(setName + "str" + i.ToString(), "Unnamed user");
            PlayerPrefs.SetInt(setName + i.ToString(), 0);
            PlayerPrefs.Save();
        }
    }

    public void SetRecord(string playerName, int record)
    {
        if (totalRecords < 10)
        {
            totalRecords = PlayerPrefs.GetInt("totalRecords", 0) + 1;
            PlayerPrefs.SetInt("totalRecords", totalRecords);
        }
        int i = CompareRecord(record);
        if (i != 10)
        {
            PlayerPrefs.SetString(setName + "str" + i.ToString(), playerName);
            PlayerPrefs.SetInt(setName + i.ToString(), record);
            PlayerPrefs.Save();
        }
    }

    public int CompareRecord(int record)
    {
        for (int i = 0; i < totalRecords; i++)
        {
            if (records[i] == 0)
            {
                return i;
            }
            if (records[i] > record)
            {
                ShiftRecords(i);
                return i;
            }
        }
        return 10;
    }

    public void ShiftRecords(int i)
    {
        for (int j = totalRecords - 1; j > i; j--)
        {
            Debug.Log(j);
            if (j != 9)
            {
                PlayerPrefs.SetString(setName + "str" + (j + 1).ToString(), PlayerPrefs.GetString(setName + j.ToString(), ""));
                PlayerPrefs.SetInt(setName + (j + 1).ToString(), PlayerPrefs.GetInt(setName + j.ToString(), 0));
                PlayerPrefs.Save();
            }
        }
    }
}
