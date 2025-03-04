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
    public DataBaseInventory data; // ������ �� ���� ������ ���������
    public List<ItemInventory> items = new List<ItemInventory>(); // ������ ��������� � ���������
    public GameObject gameObjShow; // ������ ��� ����������� �������� � ���������
    public GameObject InventoryMainObject; // �������� ������ ��������� � �����
    public int maxCount; // ������������ ���������� ��������� � ���������
    public GameObject backGround; // ��� ���������
    public bool InventoryOpen = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        backGround.SetActive(false);
        // ���� ��������� ����, ��������� �������
        if (items.Count == 0)
        {
            AddGraphics();
        }
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
                UpdateInventory(); // ��������� ���������, ���� �� ������
                GameInput.Instance.OnDisable();
                InventoryOpen = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Item_script.Instance.CloseMenu();
            backGround.SetActive(false);
            GameInput.Instance.OnEnabled();
            InventoryOpen = false;
        }
    }

    public void UseItem(int itemId)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].id == itemId && items[i].count > 0)
            {
                switch (itemId)
                {
                    case 1: // ���
                        UseFood(items[i]);
                        break;
                    case 3:
                        Healthbar.Instance.TakeDamage(20);
                        break;
                    default:
                        Debug.Log("����������� �������");
                        break;
                }
                UpdateInventory();
                return;
            }
        }
        Debug.Log("������� �� ������ ��� ���������� ����� ����");
    }

    private void UseFood(ItemInventory item)
    {
        Debug.Log("������������ ���");

        // ������ ������������� ��� (��������, �������������� ��������)
        item.count--;
        Healthbar.Instance.Heal(20);
        if (item.count <= 0)
        {
            item.id = 0; // ������� ������� �� ���������, ���� ���������� ����� 0
            Item_script itemScript = item.itemGameObj.GetComponent<Item_script>();
            itemScript.Initialize(item.id); // �������� ID � ������
        }
    }

    public void AddItem(int itemId)
    {

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
            items[i].name = itemData.name;
            if (items[i].id == 0) // ���� ������ �����
            {
                items[i].id = itemId; // ������������� ID ��������
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
