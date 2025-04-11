using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Unity.VisualScripting;
using static UnityEditor.Timeline.Actions.MenuPriority;
using static UnityEditor.Progress;
using System.ComponentModel;



public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }
    public DataBaseInventory data; // Ссылка на базу данных инвентаря
    public List<ItemInventory> items = new List<ItemInventory>(); // Список предметов в инвентаре
    public GameObject gameObjShow; // Префаб для отображения предмета в инвентаре
    public GameObject InventoryMainObject; // Основной объект инвентаря в сцене
    public int maxCount; // Максимальное количество предметов в инвентаре
    public GameObject backGround; // Фон инвентаря
    public bool InventoryOpen = false;
    public List<int> pickedItems = new List<int>(); // Список ID подобранных предметов
    public List<int> usedItems = new List<int>(); // Список ID использованных предметов
    private bool resetScene = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        backGround.SetActive(true);
        backGround.SetActive(false);
        // Если инвентарь пуст, добавляем графику
        if (items.Count == 0)
        {
            AddGraphics();
        }
    }

    public void ClearPickedItems()
    {
        pickedItems.Clear();
        usedItems.Clear();
    }

    public void ResetScene()
    {
        foreach (int itemId in pickedItems) // Удаление предметов после проигрыша
        {
            resetScene = true;
            ConsumeItem(itemId);
        }
        foreach (int itemId in usedItems) // Добавление предметов после проигрыша
        {
            resetScene = true;
            AddItem(itemId);
        }
        ClearPickedItems();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && DialogueManager.Instance.dialogPanelOpen == false)
        {
            backGround.SetActive(!backGround.activeSelf);
            GameInput.Instance.OnEnabled();
            Item_script.Instance.CloseMenu();
            InventoryOpen = false;

            if (backGround.activeSelf)
            {
                UpdateInventory(); // Обновляем инвентарь, если он открыт
                GameInput.Instance.OnDisable();
                InventoryOpen = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && InventoryOpen)
        {
            CloseInventory();
        }
    }

    private void CloseInventory()
    {
        Item_script.Instance.CloseMenu();
        backGround.SetActive(false);
        GameInput.Instance.OnEnabled();
        InventoryOpen = false;
    }

    public void UseItem(int itemId)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].id == itemId && items[i].count > 0)
            {
                ItemUseManager.Instance.UseItem(itemId, this);

                UpdateInventory(); // Обновляем отображение инвентаря
                CloseInventory();
                return;
            }
        }
        Debug.Log("Предмет не найден или количество равно нулю");
    }

    public void ConsumeItem(int itemId)
    {
        if(!resetScene)
        {
            usedItems.Add(itemId);
            resetScene = false;
        }
        
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].id == itemId && items[i].count > 0)
            {
                Consume(items[i]);
            }
        }
    }

    private void Consume(ItemInventory item)
    {
        item.count--;
        if (item.count <= 0)
        {
            item.id = 0; // Удаляем предмет из инвентаря, если количество стало 0
            Item_script itemScript = item.itemGameObj.GetComponent<Item_script>();
            itemScript.Initialize(item.id); // Передаем ID в скрипт
        }
    }

    public void AddItem(int itemId)
    {
        if (!resetScene)
        {
            pickedItems.Add(itemId);
            resetScene = false;
        }
        // Проверяем, есть ли предмет в инвентаре
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].id == itemId) // Если предмет уже есть
            {
                items[i].count++; // Увеличиваем количество
                return; // Выходим из метода
            }

        }
        // Если предмета нет, ищем первую свободную ячейку
        for (int i = 0; i < items.Count; i++)
        {
            Item itemData = data.items.Find(item => item.id == itemId);
            items[i].name = itemData.name;
            if (items[i].id == 0) // Если ячейка пуста
            {
                items[i].id = itemId; // Устанавливаем ID предмета
                items[i].count = 1; // Устанавливаем количество 1
                Item_script itemScript = items[i].itemGameObj.GetComponent<Item_script>();
                itemScript.Initialize(itemId); // Передаем ID в скрипт
                itemScript.InitializeName(itemData.name);

                return; // Выходим из метода
            }
        }
        Debug.Log("Инвентарь полон!"); // Сообщение, если инвентарь полон
    }

    // Метод для добавления графики в инвентарь
    private void AddGraphics()
    {
        for (int i = 0; i < maxCount; i++)
        {
            GameObject newItem = Instantiate(gameObjShow, InventoryMainObject.transform); // Создаем новый объект для предмета
            newItem.name = i.ToString(); // Устанавливаем имя объекта по индексу
            ItemInventory ii = new ItemInventory { itemGameObj = newItem }; // Создаем и инициализируем новый экземпляр ItemInventory
            items.Add(ii); // Добавляем новый элемент в список предметов  
        }
    }

    // Метод для обновления отображения инвентаря
    private void UpdateInventory()
    {
        for (int i = 0; i < maxCount; i++)
        {
            if (items[i].id != 0 && items[i].count > 1)
            {
                items[i].itemGameObj.GetComponentInChildren<TMP_Text>().text = items[i].count.ToString(); // Обновляем текст с количеством
            }
            else
            {
                items[i].itemGameObj.GetComponentInChildren<TMP_Text>().text = ""; // Скрываем текст, если количество 1 или ID 0
            }
            items[i].itemGameObj.GetComponentInChildren<Image>().sprite = data.items[items[i].id].img; // Обновляем изображение предмета

        }
    }

    // Метод для проверки наличия предмета
    public bool HasItem(int itemId)
    {
        foreach (var item in items)
        {
            if (item.id == itemId && item.count > 0)
            {
                return true; // Предмет найден
            }
        }
        return false; // Предмет не найден
    }
}

// Класс для представления предмета в инвентаре
[System.Serializable]
public class ItemInventory
{
    public int id; // ID предмета
    public string name;
    public GameObject itemGameObj;
    public int count; // Количество предметов

}
