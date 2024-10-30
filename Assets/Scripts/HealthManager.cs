using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public Image healthBar;

    private void Start()
    {
        health = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            Die();
        }
        UpdateHealthBar();
    }

    private void Die()
    {
        SceneManager.Instance.GameOver();
    }
    private void UpdateHealthBar()
    {
        healthBar.fillAmount = health / maxHealth;
    }
}
