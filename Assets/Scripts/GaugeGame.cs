using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeGame : MonoBehaviour
{
    public RectTransform movingBar; // ���������� �������
    public RectTransform gaugePanel; // �����
    public RectTransform targetZone; // ������� ����
    public float speed = 500f; // �������� ��������
    private bool moveRight = true; // ����������� ��������

    public bool isPlaying = false; // ���� �������
    public bool isSuccess = false; // ����� ���������

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

        // �������� �������
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
        }
        else
        {
            Debug.Log("������!");
            isSuccess = false;
        }
    }

    // ����� ��� ������� ����
    public void StartGame()
    {
        isPlaying = true;
        isSuccess = false;
    }
}