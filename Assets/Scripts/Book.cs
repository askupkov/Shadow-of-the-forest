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
    [SerializeField] Image pageImages; // ������ ����������� ��� �������
    [SerializeField] Sprite[] pageSprites;
    [SerializeField] GameObject button;
    public bool BookOpen;

    private int countPage = 3; // ���������� �������
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
        pageImages.enabled = false;
        button.SetActive(false);
    }

    private void UpdatePageText()
    {
        // ������� ��������� ���� ����� �����������
        foreach (var text in pageTexts)
        {
            text.text = "";
        }

        // ������������ ������ ������� �� ��������� �����
        for (int i = 0; i < pageTexts.Length; i++)
        {
            int pageIndex = currentPage * pageTexts.Length + i;

            if (pageIndex < storyLines.Count)
            {
                pageTexts[i].text = storyLines[pageIndex];
            }
            else
            {
                pageTexts[i].text = ""; // ������� �����, ���� ������ ��� ��������
            }

            // ��������� �����������
            pageImages.enabled = true;
            button.SetActive(true);
            pageImages.sprite = pageSprites[currentPage];
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
                if (Ritual�ircle.Instance != null)
                {
                    if (Ritual�ircle.Instance.playerInRange)
                    {
                        Ritual�ircle.Instance.startritual();
                    }
                }
                break;
        }
        BookOpen = false;
        currentPage = 0;
        BookUI.SetActive(false);
    }
}
