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
    public static DialogueManager Instance { get; private set; }
    public GameObject dialogPanel; // ������ �������
    public GameObject Panel1;
    public GameObject Panel2;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogText; // ����� �������
    public Image characterImage; // ����������� ���������

    private string[] lines = { };
    private string characterName;
    private Dictionary<string, Sprite> characterSprites = new Dictionary<string, Sprite>(); // ������� ����������� ����������


    private int index; // ������ ������� ������
    private float speedText = 0.04f; // �������� ������ ������

    private Story currentStory;
    public bool dialogPanelOpen = false;
    private bool isTyping = false; // ����, �����������, ���������� �� �����

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        dialogPanel.SetActive(false); // �������� ������ � ������
        characterSprites.Add("���������", Resources.Load<Sprite>("Characters/Svyatoslav"));
        characterSprites.Add("��������", Resources.Load<Sprite>("Characters/Guard"));
        characterSprites.Add("��������", Resources.Load<Sprite>("Characters/Starushka"));

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
        if (Inventory.Instance.InventoryOpen == false)
        {
            currentStory = new Story(inkJSON.text);
            currentStory.ChoosePathString(startingPoint);
            index = 0;
            GameInput.Instance.OnDisable();
            dialogPanelOpen = true;
            dialogPanel.SetActive(true);
            List<string> dialogLines = new List<string>();
            while (currentStory.canContinue)
            {
                string output = currentStory.Continue();
                if (!string.IsNullOrEmpty(output))
                {
                    dialogLines.Add(output);
                }
            }
            lines = dialogLines.ToArray();
            StartCoroutine(TypeLine());
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        string speakerName = GetSpeakerNameFromLine(lines[index]);

        if (string.IsNullOrEmpty(speakerName))
        {
            speakerName = characterName; // ���� ��� �� �������, ���������� ���������� ���
        }
        else
        {
            characterName = speakerName; // ��������� ��� ���������
        }

        UpdateDialogPanel(characterName);
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

    private string GetSpeakerNameFromLine(string line)
    {
        if (line.StartsWith("speaker:"))
        {
            index++;
            string[] parts = line.Split(new[] { ':' }, 2); // ��������� ������ �� ������� ":"
            if (parts.Length >= 2)
            {
                return parts[1].Substring(1).Trim(); // ��������� ��� ����� "speaker:"
            }   
        }
        return null; // ���� ��� �� �������, ���������� null
    }

    private void UpdateDialogPanel(string speakerName)
    {
        // ������������ ��� ������ �������
        Panel1.SetActive(false);
        Panel2.SetActive(false);

        // ���������� ������ ������ � ����������� �� ���������
        if (speakerName == "���������")
        {
            Panel1.SetActive(true);
            dialogText = Panel1.GetComponentInChildren<TextMeshProUGUI>(true); // ��������� ������ �� dialogText
            nameText = Panel1.transform.Find("Name").GetComponent<TextMeshProUGUI>(); // ��������� ������ �� nameText
        }
        else
        {
            Panel2.SetActive(true);
            dialogText = Panel2.GetComponentInChildren<TextMeshProUGUI>(true); // ��������� ������ �� dialogText
            nameText = Panel2.transform.Find("Name").GetComponent<TextMeshProUGUI>(); // ��������� ������ �� nameText
        }

        // ������������� ����������� ���������
        if (characterSprites.ContainsKey(speakerName) && characterImage != null)
        {
            characterImage.sprite = characterSprites[speakerName];
            characterImage.enabled = true; // �������� ����������� �����������
        }
        else if (characterImage != null)
        {
            characterImage.enabled = false; // ��������� �����������, ���� ��� �� �������
        }
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
            InspectItem.Instance.HideItem();
        }
    }
}