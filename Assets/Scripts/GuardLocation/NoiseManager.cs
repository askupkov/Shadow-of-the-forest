using UnityEngine;
using UnityEngine.UI;

public class NoiseManager : MonoBehaviour
{
    public static NoiseManager Instance { get; private set; }
    public Image noiseBarFill; // Ссылка на заполняющуюся полоску
    public float maxNoise = 10f; // Максимальный уровень шума
    private float currentNoise = 0f; // Текущий уровень шума
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

    // Увеличение уровня шума
    public void IncreaseNoise(float amount)
    {
        currentNoise += amount;
        currentNoise = Mathf.Clamp(currentNoise, 0f, maxNoise);
        UpdateNoiseBar();

        if (currentNoise >= maxNoise && !atack)
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
        atack = true;
        // Находим ближайшего стража
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

        // Активируем атаку у ближайшего стража
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
