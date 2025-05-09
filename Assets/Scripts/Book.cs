using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.UI;

public class Book : MonoBehaviour
{
    public static Book Instance { get; private set; }

    [SerializeField] Animator animator;
    [SerializeField] TextAsset inkJSON;

    public GameObject BookUI; // UI-панель с книгой
    public TextMeshProUGUI[] pageTexts; // Массив текстовых полей для страниц
    [SerializeField] Image pageImages; // Массив изображений для страниц
    [SerializeField] Sprite[] pageSprites;
    [SerializeField] GameObject button;
    public bool BookOpen;

    private int countPage = 3; // Количество страниц
    private int currentPage = 0; // Текущая страница
    private Story currentStory;
    private List<string> storyLines = new List<string>();
    private bool isAnimating = false;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        currentStory = new Story(inkJSON.text);
        CacheStoryLines();
        UpdatePageText();
        BookUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && BookOpen)
        {
            OnDisableBook();
        }
    }

    private void CacheStoryLines()
    {
        // Кэшируем все строки истории в список
        while (currentStory.canContinue)
        {
            string nextLine = currentStory.Continue().Trim();
            if (!string.IsNullOrEmpty(nextLine))
            {
                storyLines.Add(nextLine);
            }
        }
    }

    public void NextPage()
    {
        if (isAnimating) return;

        if (currentPage < countPage)
        {
            isAnimating = true;
            animator.SetTrigger("Next_page");
            currentPage++;
            StartCoroutine(AnimatePage(() => FadeInText()));
            ClearPageText();
        }
    }

    public void PreviousPage()
    {
        if (isAnimating) return;

        if (currentPage > 0)
        {
            isAnimating = true;
            animator.SetTrigger("Previous_page");
            currentPage--;
            StartCoroutine(AnimatePage(() => FadeInText()));
            ClearPageText();
        }
    }

    private void ClearPageText()
    {
        foreach (var text in pageTexts)
        {
            text.text = "";
        }
        pageImages.enabled = false;
        button.SetActive(false);
    }

    private void UpdatePageText()
    {
        // Очищаем текстовые поля перед обновлением
        foreach (var text in pageTexts)
        {
            text.text = "";
        }

        // Распределяем строки истории по текстовым полям
        for (int i = 0; i < pageTexts.Length; i++)
        {
            int pageIndex = currentPage * pageTexts.Length + i;

            if (pageIndex < storyLines.Count)
            {
                pageTexts[i].text = storyLines[pageIndex];
            }
            else
            {
                pageTexts[i].text = ""; // Очищаем текст, если больше нет контента
            }

            // Обновляем изображения
            pageImages.enabled = true;
            button.SetActive(true);
            pageImages.sprite = pageSprites[currentPage];
        }
    }

    // Анимация перелистывания страницы
    private IEnumerator AnimatePage(System.Action onComplete)
    {
        yield return new WaitForSeconds(0.4f);
        onComplete?.Invoke();

        isAnimating = false;
    }

    private void FadeInText()
    {
        UpdatePageText();
    }


    public void OnEnableBook()
    {
        BookOpen = true;
        BookUI.SetActive(true);
        StartCoroutine(InputDisabled());
    }

    private IEnumerator InputDisabled()
    {
        yield return new WaitForSeconds(0.4f);
        GameInput.Instance.OnDisable();
    }

    public void OnDisableBook()
    {
        GameInput.Instance.OnEnabled();
        BookOpen = false;
        currentPage = 0;
        UpdatePageText();
        BookUI.SetActive(false);
    }

    public void read()
    {
        switch (currentPage)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                GameInput.Instance.OnEnabled();
                if (RitualСircle.Instance != null)
                {
                    if (RitualСircle.Instance.playerInRange)
                    {
                        RitualСircle.Instance.startritual();
                    }
                }
                break;
        }
        BookOpen = false;
        currentPage = 0;
        BookUI.SetActive(false);
    }
}
