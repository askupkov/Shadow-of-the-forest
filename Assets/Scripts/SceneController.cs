using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    private bool playerInRange = false;
    public int sceneToLoad;

    public Vector2 position;
    public VectorValue playerStorage;

    private string SavePath => Path.Combine(Application.persistentDataPath, "VectorValue.json");

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
        PlayerPrefs.SetInt("LoadScene", sceneToLoad);
        PlayerPrefs.Save();
        GameInput.Instance.OnDisable(); // Отключаем ввод перед загрузкой новой сцены
        playerStorage.initialValue = position;
        SaverPosition.Instance.Save(position);
        SceneFader.Instance.FadeToLevel();
        yield return new WaitForSeconds(1f);

        Inventory.Instance.ClearPickedItems();
        Healthbar.Instance.SaveHealth();
        ItemController.Instance.SaveScene();
        SceneManager.LoadScene(sceneToLoad);
        SceneFader.Instance.FadeFromLevel();

    }

    [System.Serializable]
    public class VectorData
    {
        public float x, y, z;
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
