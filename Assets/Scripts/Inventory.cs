using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.IO;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }
    public DataBaseInventory data; // ������ �� ���� ������ ���������
    public List<ItemInventory> items = new List<ItemInventory>(); // ������ ��������� � ���������
    public GameObject gameObjShow; // ������ ��� ����������� �������� � ���������
    public GameObject InventoryMainObject; // �������� ������ ��������� � �����
    public int maxCount; // ������������ ���������� ��������� � ���������
    public GameObject backGround; // ��� ���������
    public bool InventoryOpen = false;
    public List<int> pickedItems = new List<int>(); // ������ ID ����������� ���������
    public List<int> usedItems = new List<int>(); // ������ ID �������������� ���������
    private bool resetScene = false;

    [SerializeField] GameObject notificationPanel; // ������ �����������
    [SerializeField] TextMeshProUGUI notificationText; // ����� �����������
    [SerializeField] TextMeshProUGUI notificationCountText; // ����� �����������
    [SerializeField] Image itemIcon; // ����������� �����������
    [SerializeField] Animator notificationAnimator;
    private Queue<(int itemId, string actionType)> notificationQueue = new Queue<(int, string)>(); // ������� �����������
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
        foreach (int itemId in pickedItems) // �������� ��������� ����� ���������
        {
            resetScene = true;
            ConsumeItem(itemId);
        }
        foreach (int itemId in usedItems) // ���������� ��������� ����� ���������
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
                UpdateInventory(); // ��������� ���������, ���� �� ������
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

                UpdateInventory(); // ��������� ����������� ���������
                CloseInventory();
                return;
            }
        }
        Debug.Log("������� �� ������ ��� ���������� ����� ����");
    }

    public void ConsumeItem(int itemId)
    {
        if (!resetScene)
        {
            AddToQueue(itemId, "������������");
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
            item.id = 0; // ������� ������� �� ���������, ���� ���������� ����� 0
            Item_script itemScript = item.itemGameObj.GetComponent<Item_script>();
            itemScript.Initialize(item.id); // �������� ID � ������
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
            notificationQueue.Enqueue((itemId, actionType)); // ��������� ������� � ��� �������� � �������

            // ���� ����������� �� ������������, �������� �����
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

        // ����� ��������� ������� � ��� �������� �� �������
        var nextNotification = notificationQueue.Dequeue();
        int itemId = nextNotification.itemId;
        string actionType = nextNotification.actionType;

        if (actionType == "���������" && itemId != 3 && itemId != 9)
        {
            audioSource.Play();
        }
        Item selectedItem = data.items[itemId];

        // ���������� �����������
        notificationText.text = actionType;
        itemIcon.sprite = selectedItem.img;
        notificationAnimator.SetTrigger("On");

        // ��������� �������� ��� ������� �����������
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
            AddToQueue(itemId, "���������");
            pickedItems.Add(itemId);
            resetScene = false;
        }
        // ���������, ���� �� ������� � ���������
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].id == itemId) // ���� ������� ��� ����
            {
                items[i].count++; // ����������� ����������
                return; // ������� �� ������
            }

        }
        // ���� �������� ���, ���� ������ ��������� ������
        for (int i = 0; i < items.Count; i++)
        {
            Item itemData = data.items.Find(item => item.id == itemId);
            if (items[i].id == 0) // ���� ������ �����
            {
                items[i].id = itemId; // ������������� ID ��������
                items[i].name = itemData.name;
                items[i].count = 1; // ������������� ���������� 1
                Item_script itemScript = items[i].itemGameObj.GetComponent<Item_script>();
                itemScript.Initialize(itemId); // �������� ID � ������
                itemScript.InitializeName(itemData.name);

                return; // ������� �� ������
            }
        }
        Debug.Log("��������� �����!"); // ���������, ���� ��������� �����
    }

    // ����� ��� ���������� ������� � ���������
    private void AddGraphics()
    {
        for (int i = 0; i < maxCount; i++)
        {
            GameObject newItem = Instantiate(gameObjShow, InventoryMainObject.transform); // ������� ����� ������ ��� ��������
            newItem.name = i.ToString(); // ������������� ��� ������� �� �������
            ItemInventory ii = new ItemInventory { itemGameObj = newItem }; // ������� � �������������� ����� ��������� ItemInventory
            items.Add(ii); // ��������� ����� ������� � ������ ���������  
        }
    }

    // ����� ��� ���������� ����������� ���������
    private void UpdateInventory()
    {
        for (int i = 0; i < maxCount; i++)
        {
            if (items[i].id != 0 && items[i].count > 1)
            {
                items[i].itemGameObj.GetComponentInChildren<TMP_Text>().text = items[i].count.ToString(); // ��������� ����� � �����������
            }
            else
            {
                items[i].itemGameObj.GetComponentInChildren<TMP_Text>().text = ""; // �������� �����, ���� ���������� 1 ��� ID 0
            }
            items[i].itemGameObj.GetComponentInChildren<Image>().sprite = data.items[items[i].id].img; // ��������� ����������� ��������

        }
    }

    // ����� ��� �������� ������� ��������
    public bool HasItem(int itemId)
    {
        foreach (var item in items)
        {
            if (item.id == itemId && item.count > 0)
            {
                return true; // ������� ������
            }
        }
        return false; // ������� �� ������
    }

    // ����� ��� ��������� ���������� ����������� �������� � ���������
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

    // ��������������� ����� ��� ����������
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

    // ��������� ������
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
        Debug.Log("��������� ��������: " + SavePath);
    }

    // ��������� ������
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
                itemScript.Initialize(savedItem.id); // �������� ID � ������
                itemScript.InitializeName(savedItem.name);
            }
            Debug.Log("��������� ��������");
        }
        else
        {
            Debug.Log("���� ��������� �� ������.");
        }
    }
}
// ����� ��� ������������� �������� � ���������
[System.Serializable]
public class ItemInventory
{
    public int id; // ID ��������
    public string name;
    public GameObject itemGameObj;
    public int count; // ���������� ���������
}