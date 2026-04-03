using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    public void GoToGamePlay()
    {
        SceneManager.LoadScene("GamePlay");
    }

    public void GoToTitlePage()
    {
        SceneManager.LoadScene("Title");
    }
}
