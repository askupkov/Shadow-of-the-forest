using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GaugeGame : MonoBehaviour
{
    public static GaugeGame Instance { get; private set; }
    public RectTransform movingBar; // Движущаяся полоска
    public GameObject gaugePanel; // Шкала
    private RectTransform gaugePanelRect;
    public RectTransform targetZone; // Целевая зона
    public float speed = 500f; // Скорость движения
    private bool moveRight = true; // Направление движения
    private bool GameBegun = false;

    public bool isPlaying = false; // Игра активна
    public bool isSuccess = false; // Успех попадания


    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        gaugePanel.SetActive(false);
        gaugePanelRect = gaugePanel.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && GameBegun)
        {
            StopGame();
        }

        if (!isPlaying) return;

        // Движение полоски
        float currentX = movingBar.anchoredPosition.x;
        float minX = -(gaugePanelRect.rect.width / 2) + (movingBar.rect.width / 2);
        float maxX = (gaugePanelRect.rect.width / 2) - (movingBar.rect.width / 2);

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
            Pit.Instance.start_succes();
            speed += 300f;
            GameBegun = false;
        }
        else
        {
            Debug.Log("Промах!");
            isSuccess = false;
            Pit.Instance.start_fall();
            GameBegun = false;
        }
    }

    // Метод для запуска игры
    public void StartGame()
    {
        gaugePanel.SetActive(true);
        GameBegun = true;
        isPlaying = true;
        isSuccess = false;
    }
}