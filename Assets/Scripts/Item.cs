using System.Collections;
using UnityEngine;


public class Item_script : MonoBehaviour
{
    public static Item_script Instance { get; private set; }
    [SerializeField] TextAsset inkJSON;
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
        StartCoroutine(Use());
    }

    private IEnumerator Use()
    {
        yield return new WaitForSeconds(0.1f);
        CloseMenu();
        Inventory.Instance.UseItem(itemID);
    }

    public void inspectItem()
    {
        StartCoroutine(Inspect());
    }

    private IEnumerator Inspect()
    {
        yield return new WaitForSeconds(0.1f);
        CloseMenu();
        Inventory.Instance.backGround.SetActive(false);
        Inventory.Instance.InventoryOpen = false;
        DialogueManager.Instance.StartDialog(inkJSON, itemName);
        Item selectedItem = Inventory.Instance.data.items[itemID];
        InspectItem.Instance.ShowItem(selectedItem.img_insp);
        InspectItem.Instance.Background.SetActive(false);
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

