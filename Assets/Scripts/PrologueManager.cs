using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using UnityEngine.SceneManagement;

public class PrologueManager : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] TextAsset inkJSON;

    private List<string[]> chapters = new List<string[]>();
    private float textDelay = 3f;

    private void Start()
    {
        LoadInkData();
        StartCoroutine(ShowPrologue());
    }

    private void LoadInkData()
    {
        Story story = new Story(inkJSON.text);
        List<string> currentChapter = new List<string>();
        bool isFirstLine = true;

        while (story.canContinue)
        {
            string line = story.Continue().Trim();

            if (!string.IsNullOrEmpty(line))
            {
                if (line.StartsWith("Chapter:"))
                {
                    if (!isFirstLine && currentChapter.Count > 0)
                    {
                        chapters.Add(currentChapter.ToArray());
                        currentChapter.Clear();
                    }
                    isFirstLine = false;
                }
                else
                {
                    currentChapter.Add(line);
                }
            }
        }
        if (currentChapter.Count > 0)
        {
            chapters.Add(currentChapter.ToArray());
        }
    }

    private IEnumerator ShowPrologue()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            image.sprite = sprites[i];

            if (i < chapters.Count)
            {
                foreach (string line in chapters[i])
                {
                    descriptionText.text = line;

                    SceneFader.Instance.FadeFromLevel();
                    yield return new WaitForSeconds(1f);

                    yield return new WaitForSeconds(textDelay);

                    descriptionText.text = "";
                }
            }

            SceneFader.Instance.FadeToLevel();
            yield return new WaitForSeconds(1f);
        }
        PlayerPrefs.SetInt("PrologueEnd", 1);
        PlayerPrefs.Save();
        Debug.Log("Пролог завершен");
        SceneManager.LoadScene(16);
    }
}