using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public static GameOver Instance { get; private set; }
    public GameObject gameOverPanel; // Панель с экраном смерти

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gameOverPanel.SetActive(false); // Скрываем панель при старте
    }

    public void ShowGameOverScreen()
    {
        gameOverPanel.SetActive(true); // Показываем панель при смерти
        Time.timeScale = 0f;
        AudioSetting.Instance.Mute();
    }

    public void OnRestartButton()
    {
        GameManager.Instance.RestartGame();

        Inventory.Instance.ResetScene();
        ItemController.Instance.ClearPickedItems();
        Healthbar.Instance.LoadHealth();
        GuardAI.guards.Clear();

        gameOverPanel.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
