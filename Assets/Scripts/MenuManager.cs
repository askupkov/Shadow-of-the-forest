using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] VectorValue vectorValue;
    public void StartGame()
    {
        SceneManager.LoadScene(21);
        vectorValue.initialValue = new Vector3(1.15f, 0f, 0f);
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }
}
