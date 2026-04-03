using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] Text text;
    bool isStop;
    bool done;
    int time;

    void Start()
    {
        Continue();
    }

    void Update()
    {
        if (done)
            StartCoroutine(CountUp());
    }

    public void ResetTimer()
    {
        time = 0;
        Pause();
    }

    public bool GetIsStop()
    {
        return isStop;
    }

    public IEnumerator CountUp()
    {
        done = false;
        time++;
        text.text = "Timer: " + time;

        yield return new WaitForSeconds(1);
        done = true;
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
