using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoiseManager : MonoBehaviour
{
    public static NoiseManager Instance { get; private set; }
    public Image noiseBarFill; // Ссылка на заполняющуюся полоску
    public float maxNoise = 10f; // Максимальный уровень шума
    private float currentNoise = 0f; // Текущий уровень шума


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateNoiseBar();
    }
    private void Update()
    {
        Player.Instance.Noice();
    }

    // Увеличение уровня шума
    public void IncreaseNoise(float amount)
    {
        currentNoise += amount;
        currentNoise = Mathf.Clamp(currentNoise, 0f, maxNoise);
        UpdateNoiseBar();

        if (currentNoise >= maxNoise)
        {
            OnNoiseDetected(); // Игрок замечен
        }
    }

    // Уменьшение уровня шума
    public void DecreaseNoise(float amount)
    {
        currentNoise -= amount;
        currentNoise = Mathf.Clamp(currentNoise, 0f, maxNoise);
        UpdateNoiseBar();
    }

    // Обновление визуальной шкалы
    private void UpdateNoiseBar()
    {
        // Изменяем ширину полоски
        noiseBarFill.fillAmount = currentNoise / maxNoise;

        // Изменяем цвет полоски (например, от зеленого к красному)
        Color color = Color.Lerp(Color.green, Color.red, currentNoise / maxNoise);
        noiseBarFill.color = color;
    }

    // Действие при полном заполнении шкалы шума
    private void OnNoiseDetected()
    {
        GameInput.Instance.OnDisable();
        GuardAI.Instance.atack();
    }

    public void GameOver()
    {
        Debug.Log("GameOver!");
    }
}
