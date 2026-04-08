using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    public int totalRecords;
    public int[] records = new int[10];
    private string setName = "player";

    public void ResetBoard()
    {
        totalRecords = 0;
        PlayerPrefs.SetInt("totalRecords", 0);
        for (int i = 0; i < 10; i++)
        {
            PlayerPrefs.SetString(setName + i.ToString(), "");
            PlayerPrefs.SetInt(setName + i.ToString(), 0);
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
            PlayerPrefs.SetString(setName + i.ToString(), "Unnamed user");
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
            PlayerPrefs.SetString(setName + i.ToString(), playerName);
            PlayerPrefs.SetInt(setName + i.ToString(), 0);
            PlayerPrefs.Save();
        }
        Debug.Log(totalRecords);
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
                PlayerPrefs.SetString(setName + (j + 1).ToString(), PlayerPrefs.GetString(setName + j.ToString(), ""));
                PlayerPrefs.SetInt(setName + (j + 1).ToString(), PlayerPrefs.GetInt(setName + j.ToString(), 0));
                PlayerPrefs.Save();
            }
        }
    }
}
