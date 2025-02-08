using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Healthbar : MonoBehaviour
{
    public static Healthbar Instance { get; private set; }

    Image healthBar;
    public float maxHealth = 100f; // Максимальное здоровье
    public float HP;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        healthBar = GetComponent<Image>();
        HP = maxHealth; // Устанавливаем текущее здоровье на максимальное
        UpdateHealthSlider(); // Обновляем шкалу здоровья
    }

    public void TakeDamage(int damage)
    {
        HP -= damage; // Уменьшаем текущее здоровье
        if (HP < 0) HP = 0; // Убедитесь, что здоровье не ниже 0
        UpdateHealthSlider(); // Обновляем шкалу здоровья
    }

    public void Heal(int amount)
    {
        HP += amount; // Увеличиваем текущее здоровье
        if (HP > maxHealth) HP = maxHealth; // Не превышайте максимальное здоровье
        UpdateHealthSlider(); // Обновляем шкалу здоровья
    }

    private void UpdateHealthSlider()
    {
        healthBar.fillAmount = HP / maxHealth;// Обновляем значение слайдера
    }
}


