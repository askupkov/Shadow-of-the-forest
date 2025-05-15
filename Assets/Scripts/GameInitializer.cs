using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitializer : MonoBehaviour
{
    public static GameInitializer Instance { get; private set; } 
    public GameObject persistentPrefab;
    private int scene;

    void Start()
    {
        
        Instance = this;
        // Инстанциируем персистентный объект
        Instantiate(persistentPrefab);

        if(PlayerPrefs.HasKey("LoadScene"))
        {
            scene = PlayerPrefs.GetInt("LoadScene");
        }
        else
        {
            scene = 1;
        }
        SceneManager.LoadScene(scene);
    }
}
