using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using static UnityEditor.Progress;

public class Pick_Item : MonoBehaviour
{
    public static Pick_Item Instance { get; private set; }
    [SerializeField] TextAsset inkJSON;
    public string startingPoint;
    private bool playerInRange = false;
    private BoxCollider2D Collider;
    public int itemID;
    public GameObject item;
    public Sprite itemImage;
    public bool DestroyItem = false;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Collider = GetComponent<BoxCollider2D>();
        if (PlayerPrefs.GetInt(gameObject.name, 0) == 1)
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            pick_item();
        }
    }

    public void pick_item()
    {
        Collider.enabled = false;
        DialogueManager.Instance.StartDialog(inkJSON, startingPoint);
        InspectItem.Instance.ShowItem(itemImage);
        Inventory.Instance.AddItem(itemID);
        if (DestroyItem == true)
        {
            PickUpItem();
        }
        else
        {
            PlayerPrefs.SetInt(gameObject.name, 1);
            PlayerPrefs.Save();
        }
    }

    public void PickUpItem()
    {
        PlayerPrefs.SetInt(gameObject.name, 1);
        PlayerPrefs.Save();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
