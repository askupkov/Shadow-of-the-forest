using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] GameObject Healthbar;

    void Awake()
    {
        PlayerPrefs.DeleteAll();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Уничтожаем дубликат
        }
    }
    public void DestroyPersistentObject()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
            Instance = null; // Очищаем ссылку
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Перезагрузка текущей сцены
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MenuManager");
    }

    public void OnDestroy()
    {
        Healthbar.SetActive(false);
        Destroy(Inventory.Instance);
    }
}

