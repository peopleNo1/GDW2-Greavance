using UnityEngine;
using UnityEngine.UI;

public class GamePlayControl : MonoBehaviour
{
    [Header("Health Bar Settings")]
    [SerializeField] private GameObject healthBar;
    private Image bloodBar;
    [SerializeField] private float maxHealth, width, height;

    private void Start()
    {
        bloodBar = healthBar.GetComponent<Image>();
    }

    public void setHealth(float health)
    {
        if (health <= maxHealth)
        {
            bloodBar.rectTransform.sizeDelta = new Vector2(width/maxHealth*health, height);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
