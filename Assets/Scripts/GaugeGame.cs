using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GaugeGame : MonoBehaviour
{
    public static GaugeGame Instance { get; private set; }
    public RectTransform movingBar; // Движущаяся полоска
    public GameObject panel; // Шкала
    public GameObject gaugePanel; // Панель
    public GameObject victoryPanel;
    public GameObject failPanel;
    private RectTransform gaugePanelRect;
    public RectTransform targetZone; // Целевая зона
    public float speed = 500f; // Скорость движения
    private bool moveRight = true; // Направление движения
    private bool GameBegun = false;

    private bool isPlaying = false; // Игра активна

    private float minTargetWidth = 0.06f; // Минимальная ширина целевой зоны (в процентах от ширины шкалы)
    private float maxTargetWidth = 0.15f; // Максимальная ширина целевой зоны (в процентах от ширины шкалы)


    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        gaugePanel.SetActive(false);
        gaugePanelRect = panel.GetComponent<RectTransform>();
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

        // Проверяем попадание
        if (barPosition >= targetMin && barPosition <= targetMax)
        {
            PitInside.Instance.start_succes();
            GameBegun = false;
        }
        else
        {
            PitInside.Instance.start_fall();
            GameBegun = false;
        }
    }

    // Метод для запуска игры
    public void StartGame()
    {
        gaugePanel.SetActive(true);
        GameBegun = true;
        isPlaying = true;

        RandomizeTargetZone();
    }
    private void RandomizeTargetZone()
    {
        // Рандомизация ширины целевой зоны
        float randomWidthPercentage = Random.Range(minTargetWidth, maxTargetWidth);
        float newWidth = gaugePanelRect.rect.width * randomWidthPercentage;
        targetZone.sizeDelta = new Vector2(newWidth, targetZone.sizeDelta.y);

        // Рандомизация положения целевой зоны
        float minX = -(gaugePanelRect.rect.width / 2) + (newWidth / 2);
        float maxX = (gaugePanelRect.rect.width / 2) - (newWidth / 2);
        float randomX = Random.Range(minX, maxX);
        targetZone.anchoredPosition = new Vector2(randomX, targetZone.anchoredPosition.y);
    }
}