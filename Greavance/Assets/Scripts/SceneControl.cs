using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    private static bool isStart = true;

    public void CheckStart()
    {
        if (isStart)
        {
            isStart = false;
            Debug.Log("start");
            FindObjectOfType<Leaderboard>().ResetBoard();
        }
    }

    public void GoToGamePlay()
    {
        SceneManager.LoadScene("FINALSCENE");
    }

    public void GoToTitlePage()
    {
        SceneManager.LoadScene("Title");
    }
}
