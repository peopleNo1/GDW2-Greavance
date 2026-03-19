using UnityEngine;
using UnityEngine.UI;

public class GamePlayControl : MonoBehaviour
{
    [Header("Health Bar Settings")]
    [SerializeField] private GameObject healthBar;
    [SerializeField] private float maxHealth, width, height;

    private Image bloodBar;

    private void Start()
    {
        bloodBar = healthBar.GetComponent<Image>();
    }

    public void setHealth(float health)
    {
        if (health <= maxHealth)
        {
            //healthBar.transform.localScale = new Vector2(health / 100, 1);
            bloodBar.rectTransform.sizeDelta = new Vector2(width/maxHealth*health, height);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
