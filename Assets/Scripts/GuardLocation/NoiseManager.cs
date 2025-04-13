using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoiseManager : MonoBehaviour
{
    public static NoiseManager Instance { get; private set; }
    public Image noiseBarFill; // ������ �� ������������� �������
    public float maxNoise = 10f; // ������������ ������� ����
    private float currentNoise = 0f; // ������� ������� ����


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

    // ���������� ������ ����
    public void IncreaseNoise(float amount)
    {
        currentNoise += amount;
        currentNoise = Mathf.Clamp(currentNoise, 0f, maxNoise);
        UpdateNoiseBar();

        if (currentNoise >= maxNoise)
        {
            OnNoiseDetected(); // ����� �������
        }
    }

    // ���������� ������ ����
    public void DecreaseNoise(float amount)
    {
        currentNoise -= amount;
        currentNoise = Mathf.Clamp(currentNoise, 0f, maxNoise);
        UpdateNoiseBar();
    }

    // ���������� ���������� �����
    private void UpdateNoiseBar()
    {
        // �������� ������ �������
        noiseBarFill.fillAmount = currentNoise / maxNoise;

        // �������� ���� ������� (��������, �� �������� � ��������)
        Color color = Color.Lerp(Color.green, Color.red, currentNoise / maxNoise);
        noiseBarFill.color = color;
    }

    // �������� ��� ������ ���������� ����� ����
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
