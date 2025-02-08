using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    private bool playerInRange = false;
    public string sceneToLoad;

    public Vector3 position;
    public VectorValue playerStorage;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (playerInRange)
        {
            LoadScene(sceneToLoad);
        }
    }

    public void LoadScene(string sceneToLoad)
    {
        GameInput.Instance.OnDisable(); // Отключаем ввод перед загрузкой новой сцены
        playerStorage.initialValue = position;
        SceneManager.LoadScene(sceneToLoad);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
