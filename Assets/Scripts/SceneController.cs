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
    public int sceneToLoad;

    public Vector2 position;
    public VectorValue playerStorage;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (playerInRange)
        {
            StartCoroutine(LoadScene(sceneToLoad));
        }
    }

    public void StartLoadScene(int sceneToLoad)
    {
        StartCoroutine(LoadScene(sceneToLoad));
    }

    private IEnumerator LoadScene(int sceneToLoad)
    {
        GameInput.Instance.OnDisable(); // Отключаем ввод перед загрузкой новой сцены
        playerStorage.initialValue = position;
        SceneFader.Instance.FadeToLevel();
        yield return new WaitForSeconds(1f);
        Inventory.Instance.ClearPickedItems();
        ItemController.Instance.SaveScene();
        SceneManager.LoadScene(sceneToLoad);
        SceneFader.Instance.FadeFromLevel();

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
