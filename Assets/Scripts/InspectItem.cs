using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InspectItem : MonoBehaviour
{
    public static InspectItem Instance { get; private set; }

    [SerializeField]  GameObject itemPanel; // Панель для отображения предмета
    [SerializeField] Image Image; // Изображение предмета
    public GameObject Background;

    private void Awake()
    {
        Instance = this;
        itemPanel.SetActive(false); // Скрываем панель по умолчанию
    }

    public void ShowItem(Sprite itemImage)
    {
        Image.sprite = itemImage; // Устанавливаем спрайт предмета
        itemPanel.SetActive(true); // Показываем панель
    }

    public void HideItem()
    {
        itemPanel.SetActive(false); // Скрываем панель
        Background.SetActive(true);
    }
}
