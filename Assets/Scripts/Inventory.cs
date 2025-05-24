using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.IO;

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

    [SerializeField] GameObject notificationPanel; // Панель уведомления
    [SerializeField] TextMeshProUGUI notificationText; // Текст уведомления
    [SerializeField] TextMeshProUGUI notificationCountText; // Текст уведомления
    [SerializeField] Image itemIcon; // Изображение уведомления
    [SerializeField] Animator notificationAnimator;
    private Queue<(int itemId, string actionType)> notificationQueue = new Queue<(int, string)>(); // Очередь уведомлений
    private bool isShowingNotification = false;
    private int currentItemId = 0;
    private string currentActionType = "";
    private int notificationCount = 1;
    private Coroutine hideCoroutine;
    private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
        if (items.Count == 0)
        {
            AddGraphics();
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        AudioSetting.Instance.RegisterSfx(audioSource);
        notificationCountText.text = "";
        backGround.SetActive(true);
        backGround.SetActive(false);

        LoadData();
    }

    public void ClearPickedItems()
    {
        pickedItems.Clear();
        usedItems.Clear();
        SaveData();
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
        resetScene = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !DialogueManager.Instance.dialogPanelOpen && !Pause.Instance.pauseOpen && !Book.Instance.BookOpen && !GameInput.Instance.panelOpen)
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
    }

    public void CloseInventory()
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
        if (!resetScene)
        {
            AddToQueue(itemId, "Использовано");
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

    public void AddToQueue(int itemId, string actionType)
    {
        if (isShowingNotification && currentItemId == itemId && currentActionType == actionType)
        {
            notificationCount++;
            ResetHideTimer();
            return;
        }
        else
        {
            notificationCountText.text = "";
            notificationCount = 1;
            notificationQueue.Enqueue((itemId, actionType)); // Добавляем предмет и тип действия в очередь

            // Если уведомления не показываются, начинаем показ
            if (!isShowingNotification)
            {
                ShowNextNotification();
            }
        }
        currentItemId = itemId;
        currentActionType = actionType;
    }

    private void ShowNextNotification()
    {
        if (notificationQueue.Count == 0)
        {
            isShowingNotification = false;
            return;
        }

        isShowingNotification = true;

        // Берем следующий предмет и тип действия из очереди
        var nextNotification = notificationQueue.Dequeue();
        int itemId = nextNotification.itemId;
        string actionType = nextNotification.actionType;

        if (actionType == "Добавлено" && itemId != 3 && itemId != 9)
        {
            audioSource.Play();
        }
        Item selectedItem = data.items[itemId];

        // Показываем уведомление
        notificationText.text = actionType;
        itemIcon.sprite = selectedItem.img;
        notificationAnimator.SetTrigger("On");

        // Запускаем корутину для скрытия уведомления
        hideCoroutine = StartCoroutine(HideAfterDelay());
    }

    private void ResetHideTimer()
    {
        if (notificationCount > 1)
        {
            notificationCountText.text = notificationCount.ToString();
        }
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }
        hideCoroutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        notificationAnimator.SetTrigger("Off");
        currentItemId = 0;
        currentActionType = "";

        yield return new WaitForSeconds(0.5f);
        ShowNextNotification();
    }

    public void AddItem(int itemId)
    {
        if (!resetScene)
        {
            AddToQueue(itemId, "Добавлено");
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
            if (items[i].id == 0) // Если ячейка пуста
            {
                items[i].id = itemId; // Устанавливаем ID предмета
                items[i].name = itemData.name;
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

    // Метод для получения количества конкретного предмета в инвентаре
    public int GetItemCount(int itemId)
    {
        foreach (var item in items)
        {
            if (item.id == itemId)
            {
                return item.count;
            }
        }
        return 0;
    }

    // Вспомогательный класс для сохранения
    [System.Serializable]
    public class SavedItem
    {
        public int id;
        public string name;
        public int count;
    }


    [System.Serializable]
    public class InventorySaveData
    {
        public List<SavedItem> items = new List<SavedItem>();
    }

    private string SavePath => Path.Combine(Application.persistentDataPath, "inventory.json");

    // Сохранить данные
    public void SaveData()
    {
        InventorySaveData saveData = new InventorySaveData();

        foreach (var item in items)
        {
            saveData.items.Add(new SavedItem
            {
                id = item.id,
                name = item.name,
                count = item.count
            });
        }

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(SavePath, json);
        Debug.Log("Инвентарь сохранён: " + SavePath);
    }

    // Загрузить данные
    public void LoadData()
    {
        if (File.Exists(SavePath))
        {
            string json = File.ReadAllText(SavePath);
            InventorySaveData saveData = JsonUtility.FromJson<InventorySaveData>(json);

            for (int i = 0; i < items.Count; i++)
            {
                var savedItem = saveData.items[i];

                items[i].id = savedItem.id;
                items[i].name = savedItem.name;
                items[i].count = savedItem.count;
                Item_script itemScript = items[i].itemGameObj.GetComponent<Item_script>();
                itemScript.Initialize(savedItem.id); // Передаем ID в скрипт
                itemScript.InitializeName(savedItem.name);
            }
            Debug.Log("Инвентарь загружен");
        }
        else
        {
            Debug.Log("Файл инвентаря не найден.");
        }
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