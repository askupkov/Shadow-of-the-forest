using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Healthbar : MonoBehaviour
{
    public static Healthbar Instance { get; private set; }

    Image healthBar;
    public float maxHealth = 100f;
    public float HP;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        healthBar = GetComponent<Image>();
        HP = maxHealth;
        UpdateHealthSlider();
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            HP = 0;
            Player.Instance.Die();
        }
        UpdateHealthSlider();
        
    }

    public void Heal(int amount)
    {
        HP += amount;
        if (HP > maxHealth) HP = maxHealth;
        UpdateHealthSlider();
    }

    private void UpdateHealthSlider()
    {
        healthBar.fillAmount = HP / maxHealth;
    }
}


