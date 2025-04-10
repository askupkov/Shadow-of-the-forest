using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GaugeGame : MonoBehaviour
{
    public static GaugeGame Instance { get; private set; }
    public RectTransform movingBar; // ���������� �������
    public GameObject panel; // �����
    public GameObject gaugePanel; // ������
    public GameObject victoryPanel;
    public GameObject failPanel;
    private RectTransform gaugePanelRect;
    public RectTransform targetZone; // ������� ����
    public float speed = 500f; // �������� ��������
    private bool moveRight = true; // ����������� ��������
    private bool GameBegun = false;

    private bool isPlaying = false; // ���� �������

    private float minTargetWidth = 0.06f; // ����������� ������ ������� ���� (� ��������� �� ������ �����)
    private float maxTargetWidth = 0.15f; // ������������ ������ ������� ���� (� ��������� �� ������ �����)


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

        // �������� �������
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

    // ����� ��� ��������� ���� � �������� ���������
    public void StopGame()
    {
        isPlaying = false;

        // �������� ������� ������� �������
        float barPosition = movingBar.anchoredPosition.x;

        // �������� ������� ������� ����
        float targetMin = targetZone.anchoredPosition.x - targetZone.rect.width / 2;
        float targetMax = targetZone.anchoredPosition.x + targetZone.rect.width / 2;

        // ��������� ���������
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

    // ����� ��� ������� ����
    public void StartGame()
    {
        gaugePanel.SetActive(true);
        GameBegun = true;
        isPlaying = true;

        RandomizeTargetZone();
    }
    private void RandomizeTargetZone()
    {
        // ������������ ������ ������� ����
        float randomWidthPercentage = Random.Range(minTargetWidth, maxTargetWidth);
        float newWidth = gaugePanelRect.rect.width * randomWidthPercentage;
        targetZone.sizeDelta = new Vector2(newWidth, targetZone.sizeDelta.y);

        // ������������ ��������� ������� ����
        float minX = -(gaugePanelRect.rect.width / 2) + (newWidth / 2);
        float maxX = (gaugePanelRect.rect.width / 2) - (newWidth / 2);
        float randomX = Random.Range(minX, maxX);
        targetZone.anchoredPosition = new Vector2(randomX, targetZone.anchoredPosition.y);
    }
}