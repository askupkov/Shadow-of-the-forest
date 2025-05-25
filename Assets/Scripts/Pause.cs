using System.Collections;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public static Pause Instance { get; private set; }
    [SerializeField] GameObject pausePanel;
    public bool pauseOpen = false;

    private void Awake()
    {
        Instance = this;
        pausePanel.SetActive(false);
    }

    public void managePause()
    {
        if(GameInput.Instance != null)
        {
            GameInput.Instance.OnEnabled();
        }

        pausePanel.SetActive(!pausePanel.activeSelf);
        pauseOpen = false;
        Time.timeScale = 1f;
        AudioSetting.Instance.UnMute();

        if (pausePanel.activeSelf)
        {
            if (GameInput.Instance != null)
            {
                GameInput.Instance.OnDisable();
            }
            pauseOpen = true;
            Time.timeScale = 0f; // Остановка времени
            AudioSetting.Instance.Mute();
        }
    }

    public void openSetting()
    {
        pausePanel.SetActive(false);
        pauseOpen = false;
        AudioSetting.Instance.openAudioSetting();
    }

    public void openManagement()
    {
        Management.Instance.OpenManagement();
    }

    public void exitGame()
    {
        StartCoroutine(Exit());
    }
    private IEnumerator Exit()
    {
        yield return new WaitForSeconds(0.1f);
        Application.Quit();
    }
}
