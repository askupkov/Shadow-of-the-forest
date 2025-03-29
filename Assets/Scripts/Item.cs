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
    public TextAsset inkJSON;
    public int itemID;
    public string itemName;
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
        Inventory.Instance.backGround.SetActive(false);
        Inventory.Instance.InventoryOpen = false;
        DialogueManager.Instance.StartDialog(inkJSON, itemName);
    }

    public void Initialize(int id)
    {
        itemID = id;
    }

    public void InitializeName(string name)
    {
        itemName = name;
    }

    private void ShowActionMenu(bool show)
    {
        // ���� �� ����� ������� ������� ����
        if (show)
        {
            // ��������� ���������� �������� ����, ���� ��� ���������� � �� �������� �������
            if (currentActiveItem != null && currentActiveItem != this)
            {
                currentActiveItem.ShowActionMenu(false);
            }

            // ������������� ������� �������� ����
            currentActiveItem = this;
        }
        else
        {
            // ���� ��������� ������� ����, ���������� ������� �������� �������
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
        // ����������� ��������� ����
        ShowActionMenu(!interactionButtons.gameObject.activeSelf);
    }

    public void CloseMenu()
    {
        // ��������� ������� ����, ���� ��� �������
        if (currentActiveItem != null)
        {
            currentActiveItem.ShowActionMenu(false);
        }
    }
}

