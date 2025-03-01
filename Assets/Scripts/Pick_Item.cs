using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Pick_Item : MonoBehaviour
{
    public TextAsset inkJSON;
    public string startingPoint;
    private bool playerInRange = false;
    private DialogueManager dialogueManager;
    private BoxCollider2D Collider;
    public int itemID;
    public GameObject item;

    private void Start()
    {
        Collider = GetComponent<BoxCollider2D>();
        dialogueManager = FindObjectOfType<DialogueManager>();
    }
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Collider.enabled = false;
            dialogueManager.StartDialog(inkJSON, startingPoint);
            Inventory.Instance.AddItem(itemID);
            item.GetComponent<ItemStateManager>().PickUpItem();
        }
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
