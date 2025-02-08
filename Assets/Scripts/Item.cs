using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class Item_script : MonoBehaviour
{
    public static Item_script Instance { get; private set; }
    public int itemID;
    public Canvas interactionButtons;
    private static Item_script currentActiveItem;
    

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        interactionButtons.gameObject.SetActive(false);
    }
    private void Update()
    {

    }

    public void UseItem()
    {
        Item_script.Instance.CloseMenu();
        Inventory.Instance.UseItem(itemID);
    }

    public void InspectItem()
    {
        Item_script.Instance.CloseMenu();
    }

    public void Initialize(int id)
    {
        itemID = id;
    }

    private void ShowActionMenu(bool show)
    {
        // Если мы хотим открыть текущее меню
        if (show)
        {
            // Закрываем предыдущее активное меню, если оно существует и не является текущим
            if (currentActiveItem != null && currentActiveItem != this)
            {
                currentActiveItem.ShowActionMenu(false);
            }

            // Устанавливаем текущее активное меню
            currentActiveItem = this;
        }
        else
        {
            // Если закрываем текущее меню, сбрасываем текущий активный элемент
            if (currentActiveItem == this)
            {
                currentActiveItem = null;
            }
        }
        if (itemID != 0)
        {
            interactionButtons.gameObject.SetActive(show);
        }
    }

    public void OnButtonClick()
    {
        // Переключаем состояние меню
        ShowActionMenu(!interactionButtons.gameObject.activeSelf);
    }

    public void CloseMenu()
    {
        // Закрываем текущее меню, если оно открыто
        if (currentActiveItem != null)
        {
            currentActiveItem.ShowActionMenu(false);
        }
    }
}

