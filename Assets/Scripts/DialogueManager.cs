using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using UnityEditor.Rendering;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogPanel; // ������ �������
    public GameObject Panel1; // ������ �������
    public GameObject Panel2; // ������ �������
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogText; // ����� �������
    public Image characterImage; // ����������� ���������

    private string[] lines = { };
    private string characterName;
    private Sprite currentCharacterSprite; // ������� ����������� ���������
    private Dictionary<string, Sprite> characterSprites = new Dictionary<string, Sprite>(); // ������� ����������� ����������


    private int index; // ������ ������� ������
    private float speedText = 0.06f; // �������� ������ ������

    private Story currentStory;
    public bool dialogPanelOpen = false;
    private bool isTyping = false; // ����, �����������, ���������� �� �����


    private void Start()
    {
        dialogPanel.SetActive(false); // �������� ������ � ������
        characterSprites.Add("���������", Resources.Load<Sprite>("Characters/Svyatoslav"));
        characterSprites.Add("��������", Resources.Load<Sprite>("Characters/Guard"));

        // ���������, ��� ��� ������� ���������
        foreach (var kvp in characterSprites)
        {
            if (kvp.Value == null)
            {
                Debug.LogError($"����������� ��� ��������� '{kvp.Key}' �� �������!");
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && dialogPanelOpen == true)
        {
            SkipTextClick();
        }
    }

    public void StartDialog(TextAsset inkJSON, string startingPoint)
    {
        currentStory = new Story(inkJSON.text);
        currentStory.ChoosePathString(startingPoint);
        index = 0;
        GameInput.Instance.OnDisable();
        dialogPanelOpen = true;
        dialogPanel.SetActive(true);
        dialogText.text = "";
        nameText.text = "";
        List<string> dialogLines = new List<string>();
        while (currentStory.canContinue)
        {
            string output = currentStory.Continue();
            if (!string.IsNullOrEmpty(output))
            {
                if (output.StartsWith("speaker:"))
                {
                    // ��������� ��� ���������
                    string[] parts = output.Split(new[] { ':' }, 2); // ��������� ������ �� ������� "\n"
                    characterName = parts[1].Substring(1).Trim();
                    // ������������� ����������� ���������
                    if (characterSprites.ContainsKey(characterName))
                    {
                        currentCharacterSprite = characterSprites[characterName];
                    }
                }
                else
                {
                    dialogLines.Add(output);
                }
                if(characterName == "���������")
                {
                    Panel1.SetActive(true);
                    Panel2.SetActive(false);
                    dialogText = Panel1.GetComponentInChildren<TextMeshProUGUI>(true);
                    nameText = Panel1.transform.Find("Name").GetComponent<TextMeshProUGUI>();
                }
                else
                {
                    Panel1.SetActive(false);
                    Panel2.SetActive(true);
                    dialogText = Panel2.GetComponentInChildren<TextMeshProUGUI>(true);
                    nameText = Panel2.transform.Find("Name").GetComponent<TextMeshProUGUI>();
                    characterImage.sprite = currentCharacterSprite;
                }
            }
        }
        lines = dialogLines.ToArray();
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