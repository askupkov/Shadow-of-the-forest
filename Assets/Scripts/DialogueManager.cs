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
    public GameObject dialogPanel; // Панель диалога
    public GameObject Panel1; // Панель диалога
    public GameObject Panel2; // Панель диалога
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogText; // Текст диалога
    public Image characterImage; // Изображение персонажа

    private string[] lines = { };
    private string characterName;
    private Sprite currentCharacterSprite; // Текущее изображение персонажа
    private Dictionary<string, Sprite> characterSprites = new Dictionary<string, Sprite>(); // Словарь изображений персонажей


    private int index; // Индекс текущей строки
    private float speedText = 0.06f; // Скорость вывода текста

    private Story currentStory;
    public bool dialogPanelOpen = false;
    private bool isTyping = false; // Флаг, указывающий, печатается ли текст


    private void Start()
    {
        dialogPanel.SetActive(false); // Скрываем панель в начале
        characterSprites.Add("Святослав", Resources.Load<Sprite>("Characters/Svyatoslav"));
        characterSprites.Add("Стражник", Resources.Load<Sprite>("Characters/Guard"));

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
                    // Извлекаем имя персонажа
                    string[] parts = output.Split(new[] { ':' }, 2); // Разделяем строку по символу "\n"
                    characterName = parts[1].Substring(1).Trim();
                    // Устанавливаем изображение персонажа
                    if (characterSprites.ContainsKey(characterName))
                    {
                        currentCharacterSprite = characterSprites[characterName];
                    }
                }
                else
                {
                    dialogLines.Add(output);
                }
                if(characterName == "Святослав")
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
        }
    }
}