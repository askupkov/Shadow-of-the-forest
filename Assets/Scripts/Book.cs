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

    public GameObject BookUI; // UI-������ � ������
    public TextMeshProUGUI[] pageTexts; // ������ ��������� ����� ��� �������
    [SerializeField] Image[] pageImages; // ������ ����������� ��� �������
    [SerializeField] Sprite[] pageSprites;
    public bool BookOpen;

    float fadeDuration = 0.15f;

    private int countPage = 5; // ���������� �������
    private int currentPage = 0; // ������� ��������
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
        OnDisableBook();
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
        // �������� ��� ������ ������� � ������
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
        foreach (var image in pageImages)
        {
            image.sprite = null;
            image.enabled = false;
        }
    }

    private void UpdatePageText()
    {
        for (int i = 0; i < pageTexts.Length; i++)
        {
            pageTexts[i].text = "";
            int pageIndex = currentPage * pageTexts.Length + i;
            if (pageIndex < storyLines.Count)
            {
                pageTexts[i].text += storyLines[pageIndex];
            }
            else
            {
                pageTexts[i].text = ""; // ������� �����, ���� ������ ��� ��������
            }
            // ��������� �����������
            //if (pageIndex < pageSprites.Length && pageSprites[pageIndex] != null)
            //{
            //    pageImages[i].sprite = pageSprites[pageIndex];
            //    pageImages[i].enabled = true; // �������� �����������
            //}
            //else
            //{
            //    pageImages[i].enabled = false; // ��������� �����������, ���� ��� ���
            //}
        }
    }

    // �������� �������������� ��������
    private IEnumerator AnimatePage(System.Action onComplete)
    {
        yield return new WaitForSeconds(0.4f);
        onComplete?.Invoke();

        isAnimating = false;
    }

    private void FadeInText()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        UpdatePageText(); // ��������� ������ �� ���������

        // ��������� ��������� ������������ ������
        foreach (var text in pageTexts)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
        }

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);

            foreach (var text in pageTexts)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            }

            yield return null;
        }

        // ��������, ��� ����� ��������� ����� � �����
        foreach (var text in pageTexts)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);
        }
    }

    public void OnEnableBook()
    {
        BookOpen = true;
        BookUI.SetActive(true);
    }

    public void OnDisableBook()
    {
        BookOpen = false;
        currentPage = 0;
        BookUI.SetActive(false);
    }

    public void read()
    {
        if(Ritual�ircle.Instance != null)
        {
            Ritual�ircle.Instance.startritual();
        }
    }
}
