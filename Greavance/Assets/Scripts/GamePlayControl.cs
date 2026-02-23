using UnityEngine;
using UnityEngine.UI;

public class GamePlayControl : MonoBehaviour
{
    //health bar things
    [SerializeField] private GameObject healthBar;
    private Image bloodBar;
    [SerializeField] private float maxHealth, width, height;

    //setting things
    [SerializeField] private Button guide;
    private bool onSetting = false;
    [SerializeField] GameObject panel;
    [SerializeField] private GameObject settingPage;
    [SerializeField] private GameObject close;
    [SerializeField] private GameObject quit;

    private void Start()
    {
        bloodBar = healthBar.GetComponent<Image>();
        setHealth(50);
    }

    public void setHealth(float health)
    {
        if (health <= maxHealth)
        {
            bloodBar.rectTransform.sizeDelta = new Vector2(width/maxHealth*health, height);
        }
    }
    
    public void ClickSetting()
    {
        guide.enabled = onSetting;
        onSetting = !onSetting;
        panel.SetActive(onSetting);
        settingPage.SetActive(onSetting);
        close.SetActive(onSetting);
        quit.SetActive(onSetting);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
