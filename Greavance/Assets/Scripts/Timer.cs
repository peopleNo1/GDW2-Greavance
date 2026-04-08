using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] Text text;
    bool isStop;
    bool done;
    int time;
    private Coroutine coroutine;

    void Start()
    {
        time = PlayerPrefs.GetInt("totalRecords", 0);
        Debug.Log(time);
        Continue();
    }

    void Update()
    {
        if (done)
            coroutine = StartCoroutine(CountUp());
    }

    public void ResetTimer()
    {
        time = 0;
        PlayerPrefs.SetInt("totalRecords", 0);
        PlayerPrefs.Save();
    }

    public bool GetIsStop()
    {
        return isStop;
    }

    public IEnumerator CountUp()
    {
        done = false;
        time++;
        PlayerPrefs.SetInt("totalRecords", time);
        PlayerPrefs.Save();
        text.text = "Timer: " + time;

        yield return new WaitForSeconds(1);
        done = true;
    }

    public void StopTiming()
    {
        StopCoroutine(coroutine);
        SetDone(false);
    }

    public void SetDone(bool isDone)
    {
        done = isDone;
    }

    public void Pause()
    {
        isStop = true;
        Time.timeScale = 0;
    }

    public void Continue()
    {
        isStop = false;
        Time.timeScale = 1;
    }
}
