using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shield : MonoBehaviour
{
    private int _currentHealth;
    private Regenerate _regenerateAbility;

    public void Initialize(int health, Regenerate ability)
    {
        _currentHealth = health;
        _regenerateAbility = ability;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) //Change this to the player's attack tag.
        {
            float damage = 10f; // Get this from player's attack damage

            TakeDamage(damage);
        }
    }

    private void TakeDamage(float damage)
    {
        _currentHealth -= (int) damage;

        if (_currentHealth <= 0)
        {
            DestroyShield();
        }
    }

    private void DestroyShield()
    {
        if (_regenerateAbility != null)
        {
            _regenerateAbility.OnShieldDestroy();
        }

        Destroy(gameObject);
    }
}
