using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GaugeGame : MonoBehaviour
{
    public static GaugeGame Instance { get; private set; }
    public RectTransform movingBar; // ���������� �������
    public GameObject gaugePanel; // �����
    private RectTransform gaugePanelRect;
    public RectTransform targetZone; // ������� ����
    public float speed = 500f; // �������� ��������
    private bool moveRight = true; // ����������� ��������
    private bool GameBegun = false;

    public bool isPlaying = false; // ���� �������
    public bool isSuccess = false; // ����� ���������


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

        // �������: ������� �������� � �������
        Debug.Log($"Bar Position: {barPosition}");
        Debug.Log($"Target Min: {targetMin}, Target Max: {targetMax}");

        // ��������� ���������
        if (barPosition >= targetMin && barPosition <= targetMax)
        {
            Debug.Log("�����!");
            isSuccess = true;
            Pit.Instance.start_succes();
            speed += 300f;
            GameBegun = false;
        }
        else
        {
            Debug.Log("������!");
            isSuccess = false;
            Pit.Instance.start_fall();
            GameBegun = false;
        }
    }

    // ����� ��� ������� ����
    public void StartGame()
    {
        gaugePanel.SetActive(true);
        GameBegun = true;
        isPlaying = true;
        isSuccess = false;
    }
}