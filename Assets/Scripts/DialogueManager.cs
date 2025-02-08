using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
  

    public GameObject dialogPanel; // ������ �������
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogText; // ����� �������
    public string[] lines = { };
    public string characterName;
    private int index; // ������ ������� ������
    private float speedText = 0.06f; // �������� ������ ������

    public bool dialogPanelOpen = false;
    private bool isTyping = false; // ����, �����������, ���������� �� �����


    private void Start()
    {
        dialogPanel.SetActive(false); // �������� ������ � ������
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && dialogPanelOpen == true)
        {
            SkipTextClick();
        }
    }
    public void StartDialog()
    {
        //dialogText.text = ""; // ������� ����� ����� ������� ������
        index = 0;
        GameInput.Instance.OnDisable();
        dialogPanel.SetActive(true); // ���������� ������
        dialogPanelOpen = true;
       
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogText.text = ""; // ������� ����� ����� ������� ������
        nameText.text = characterName;

        foreach (char c in lines[index].ToCharArray())
        {
            dialogText.text += c;
            yield return new WaitForSeconds(speedText);
        }
        isTyping = false;
    }

    public void SkipTextClick()
    {
        if (isTyping) // ���� ����� ����������
        {
            StopAllCoroutines(); // ������������� ������ ������
            dialogText.text = lines[index]; // ���������� ��� ������ �����
            isTyping = false;
        }
        else
        {
            NextLines(); // ��������� � ��������� ������
        }
    }


    private void NextLines()
    {
        if (index < lines.Length - 1)
        {
            index++;
            StartCoroutine(TypeLine()); // �������� ��������� ������
        }
        else
        {
            dialogPanel.SetActive(false); // ��������� ������, ���� ������ �����������
            dialogPanelOpen = false;
            GameInput.Instance.OnEnabled(); // �������� ����������

        }
    }
}