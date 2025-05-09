using UnityEngine;
using UnityEngine.UI;

public class NoiseManager : MonoBehaviour
{
    public static NoiseManager Instance { get; private set; }
    public Image noiseBarFill; // ������ �� ������������� �������
    public float maxNoise = 10f; // ������������ ������� ����
    private float currentNoise = 0f; // ������� ������� ����
    public bool atack;

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

        if (currentNoise >= maxNoise && !atack)
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
        atack = true;
        // ������� ���������� ������
        GuardAI closestGuard = null;
        float closestDistance = float.MaxValue;

        foreach (var guard in GuardAI.guards)
        {
            if (guard == null || !guard.gameObject.activeInHierarchy)
            {
                continue;
            }

            float distance = Vector3.Distance(guard.transform.position, Player.Instance.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestGuard = guard;
            }
        }

        // ���������� ����� � ���������� ������
        if (closestGuard != null)
        {
            closestGuard.atack();
        }
    }

    public void GameOver()
    {
        Debug.Log("GameOver!");
    }
}
