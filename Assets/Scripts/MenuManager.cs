using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using static SceneController;

public class Menu : MonoBehaviour
{
    [SerializeField] VectorValue vectorValue;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        AudioSetting.Instance.RegisterMusic(audioSource);
    }

    public void NewGame()
    {
        StartCoroutine(New());
    }

    private IEnumerator New()
    {
        yield return new WaitForSeconds(0.1f);
        PlayerPrefs.DeleteAll();
        SaverPosition.Instance.DestroySave();
        AudioSetting.Instance.saveAudio();
        vectorValue.initialValue = new Vector3(1.15f, 0f, 0f);
        SceneManager.LoadScene(21);
    }

    public void Continue()
    {
        StartCoroutine(CoroutineContinue());
    }

    private IEnumerator CoroutineContinue()
    {
        yield return new WaitForSeconds(0.1f);
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

    public void Setting()
    {
        AudioSetting.Instance.openAudioSetting();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
