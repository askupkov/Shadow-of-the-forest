using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeGame : MonoBehaviour
{
    public RectTransform movingBar; // Движущаяся полоска
    public RectTransform gaugePanel; // Шкала
    public RectTransform targetZone; // Целевая зона
    public float speed = 500f; // Скорость движения
    private bool moveRight = true; // Направление движения

    public bool isPlaying = false; // Игра активна
    public bool isSuccess = false; // Успех попадания

    void Start()
    {
        StartGame();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StopGame();
        }

        if (!isPlaying) return;

        // Движение полоски
        float currentX = movingBar.anchoredPosition.x;
        float minX = -(gaugePanel.rect.width / 2) + (movingBar.rect.width / 2);
        float maxX = (gaugePanel.rect.width / 2) - (movingBar.rect.width / 2);

        if (moveRight)
        {
            currentX += speed * Time.deltaTime;
            if (currentX >= maxX)
            {
                moveRight = false;
            }
        }
        else
        {
            currentX -= speed * Time.deltaTime;
            if (currentX <= minX)
            {
                moveRight = true;
            }
        }

        movingBar.anchoredPosition = new Vector2(currentX, movingBar.anchoredPosition.y);
    }

    // Метод для остановки игры и проверки попадания
    public void StopGame()
    {
        isPlaying = false;

        // Получаем текущую позицию полоски
        float barPosition = movingBar.anchoredPosition.x;

        // Получаем границы целевой зоны
        float targetMin = targetZone.anchoredPosition.x - targetZone.rect.width / 2;
        float targetMax = targetZone.anchoredPosition.x + targetZone.rect.width / 2;

        // Отладка: выводим значения в консоль
        Debug.Log($"Bar Position: {barPosition}");
        Debug.Log($"Target Min: {targetMin}, Target Max: {targetMax}");

        // Проверяем попадание
        if (barPosition >= targetMin && barPosition <= targetMax)
        {
            Debug.Log("Успех!");
            isSuccess = true;
        }
        else
        {
            Debug.Log("Промах!");
            isSuccess = false;
        }
    }

    // Метод для запуска игры
    public void StartGame()
    {
        isPlaying = true;
        isSuccess = false;
    }
}