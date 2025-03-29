using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Book : MonoBehaviour
{
    [SerializeField] Animator animator;
    public GameObject BookUI; // UI-������ � ������
    public TextMeshProUGUI[] pageTexts; // ������ ��������� ����� ��� �������
    [SerializeField] TextMeshProUGUI LeftPage;
    [SerializeField] TextMeshProUGUI RightPage;
    public string[] bookContent; // ������ ����� � ���������� �����

    private int currentPage = 0; // ������� ��������

    private void Start()
    {
        UpdatePageText();
    }

    public void NextPage()
    {
        animator.SetTrigger("Next_page");
        currentPage++;
        StartCoroutine(AnimatePage(() => UpdatePageText()));
        LeftPage.text = "";
        RightPage.text = "";
    }

    public void PreviousPage()
    {
        animator.SetTrigger("Previous_page");
        currentPage--;
        StartCoroutine(AnimatePage(() => UpdatePageText()));
        LeftPage.text = "";
        RightPage.text = "";
    }

    private void UpdatePageText()
    {
        for (int i = 0; i < pageTexts.Length; i++)
        {
            int pageIndex = currentPage * pageTexts.Length + i;
            pageTexts[i].text = bookContent[pageIndex];
            if (pageIndex < bookContent.Length)
            {
                pageTexts[i].text = bookContent[pageIndex];
            }
            else
            {
                pageTexts[i].text = ""; // ������� �����, ���� �������� ������
            }
        }
    }

    // �������� �������������� ��������
    private IEnumerator AnimatePage(System.Action onComplete)
    {
        yield return new WaitForSeconds(0.5f);
        onComplete?.Invoke();
    }
}
