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
    public GameObject dialogPanel; // Панель диалога
    public GameObject Panel1;
    public GameObject Panel2;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogText; // Текст диалога
    public Image characterImage; // Изображение персонажа

    private string[] lines = { };
    private string characterName;
    private Dictionary<string, Sprite> characterSprites = new Dictionary<string, Sprite>(); // Словарь изображений персонажей


    private int index; // Индекс текущей строки
    private float speedText = 0.04f; // Скорость вывода текста

    private Story currentStory;
    public bool dialogPanelOpen = false;
    private bool isTyping = false; // Флаг, указывающий, печатается ли текст

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        dialogPanel.SetActive(false); // Скрываем панель в начале
        characterSprites.Add("Святослав", Resources.Load<Sprite>("Characters/Svyatoslav"));
        characterSprites.Add("Стражник", Resources.Load<Sprite>("Characters/Guard"));
        characterSprites.Add("Старушка", Resources.Load<Sprite>("Characters/Starushka"));

        // Проверяем, что все спрайты загружены
        foreach (var kvp in characterSprites)
        {
            if (kvp.Value == null)
            {
                Debug.LogError($"Изображение для персонажа '{kvp.Key}' не найдено!");
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
            speakerName = characterName; // Если имя не указано, используем предыдущее имя
        }
        else
        {
            characterName = speakerName; // Обновляем имя персонажа
        }

        UpdateDialogPanel(characterName);
        dialogText.text = ""; // Очищаем текст перед началом печати
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
            string[] parts = line.Split(new[] { ':' }, 2); // Разделяем строку по символу ":"
            if (parts.Length >= 2)
            {
                return parts[1].Substring(1).Trim(); // Извлекаем имя после "speaker:"
            }   
        }
        return null; // Если имя не указано, возвращаем null
    }

    private void UpdateDialogPanel(string speakerName)
    {
        // Деактивируем все панели диалога
        Panel1.SetActive(false);
        Panel2.SetActive(false);

        // Активируем нужную панель в зависимости от персонажа
        if (speakerName == "Святослав")
        {
            Panel1.SetActive(true);
            dialogText = Panel1.GetComponentInChildren<TextMeshProUGUI>(true); // Обновляем ссылку на dialogText
            nameText = Panel1.transform.Find("Name").GetComponent<TextMeshProUGUI>(); // Обновляем ссылку на nameText
        }
        else
        {
            Panel2.SetActive(true);
            dialogText = Panel2.GetComponentInChildren<TextMeshProUGUI>(true); // Обновляем ссылку на dialogText
            nameText = Panel2.transform.Find("Name").GetComponent<TextMeshProUGUI>(); // Обновляем ссылку на nameText
        }

        // Устанавливаем изображение персонажа
        if (characterSprites.ContainsKey(speakerName) && characterImage != null)
        {
            characterImage.sprite = characterSprites[speakerName];
            characterImage.enabled = true; // Включаем отображение изображения
        }
        else if (characterImage != null)
        {
            characterImage.enabled = false; // Отключаем изображение, если оно не найдено
        }
    }

    public void SkipTextClick()
    {
        if (isTyping) // Если текст печатается
        {
            StopAllCoroutines(); // Останавливаем печать текста
            dialogText.text = lines[index]; // Показываем всю строку сразу
            isTyping = false;
        }
        else
        {
            NextLines(); // Переходим к следующей строке
        }
    }

    private void NextLines()
    {
        if (index < lines.Length - 1)
        {
            index++;
            StartCoroutine(TypeLine()); // Печатаем следующую строку
        }
        else
        {
            dialogPanel.SetActive(false); // Закрываем панель, если строки закончились
            dialogPanelOpen = false;
            GameInput.Instance.OnEnabled(); // Включаем управление
            InspectItem.Instance.HideItem();
        }
    }
}