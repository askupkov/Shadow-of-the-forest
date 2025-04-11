using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public static GameOver Instance { get; private set; }
    public GameObject gameOverPanel; // ������ � ������� ������

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gameOverPanel.SetActive(false); // �������� ������ ��� ������
    }

    public void ShowGameOverScreen()
    {
        gameOverPanel.SetActive(true); // ���������� ������ ��� ������
    }

    public void OnRestartButton()
    {
        GameManager.Instance.RestartGame();
        Inventory.Instance.ResetScene();
        ItemController.Instance.ClearPickedItems();
        gameOverPanel.SetActive(false);
    }

    public void OnMainMenuButton()
    {
        GameManager.Instance.ReturnToMainMenu();
    }
}
