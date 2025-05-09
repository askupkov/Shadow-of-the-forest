using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitializer : MonoBehaviour
{
    public static GameInitializer Instance { get; private set; } 
    public GameObject persistentPrefab;
    [SerializeField] int scene;

    void Start()
    {
        Instance = this;
        // Инстанциируем персистентный объект
        Instantiate(persistentPrefab);

        SceneManager.LoadScene(scene);
    }
}
