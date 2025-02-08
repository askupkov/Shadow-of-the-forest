using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
  

    public GameObject dialogPanel; // Панель диалога
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogText; // Текст диалога
    public string[] lines = { };
    public string characterName;
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
    public void StartDialog()
    {
        //dialogText.text = ""; // Очищаем текст перед началом печати
        index = 0;
        GameInput.Instance.OnDisable();
        dialogPanel.SetActive(true); // Показываем панель
        dialogPanelOpen = true;
       
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