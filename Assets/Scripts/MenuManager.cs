using UnityEngine;
using UnityEngine.SceneManagement;
using static SceneController;

public class Menu : MonoBehaviour
{
    [SerializeField] VectorValue vectorValue;
    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        SaverPosition.Instance.DestroySave();
        vectorValue.initialValue = new Vector3(1.15f, 0f, 0f);
        SceneManager.LoadScene(21);
    }
    public void Continue()
    {
        if (PlayerPrefs.GetInt("PrologueEnd", 0) == 1)
        {
            SceneManager.LoadScene(16);
        }
        else
        {
            vectorValue.initialValue = new Vector3(1.15f, 0f, 0f);
            SceneManager.LoadScene(21);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
