using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DialogueData
{
    public string name;
    public List<string> dialog;
}

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogPanel; // ������ �������
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogText; // ����� �������
    private string[] lines = { };
    private string characterName;
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
    public void StartDialog(string[] dialogLines, string charName)
    {
        lines = dialogLines;
        characterName = charName;
        index = 0;
        GameInput.Instance.OnDisable();
        dialogPanel.SetActive(true);
        dialogPanelOpen = true;
        StartCoroutine(TypeLine());
    }

    public void LoadDialogueFromJSON(TextAsset jsonFile)
    {
        DialogueData data = JsonUtility.FromJson<DialogueData>(jsonFile.text);
        StartDialog(data.dialog.ToArray(), data.name);
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

            float randomDelay = UnityEngine.Random.Range(0f, 0.02f);
            yield return new WaitForSeconds(randomDelay);
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