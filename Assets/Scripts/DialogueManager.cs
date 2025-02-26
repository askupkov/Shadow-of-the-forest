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
    public GameObject dialogPanel; // Панель диалога
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogText; // Текст диалога
    private string[] lines = { };
    private string characterName;
    private int index; // Индекс текущей строки
    private float speedText = 0.06f; // Скорость вывода текста

    public bool dialogPanelOpen = false;
    private bool isTyping = false; // Флаг, указывающий, печатается ли текст


    private void Start()
    {
        dialogPanel.SetActive(false); // Скрываем панель в начале
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