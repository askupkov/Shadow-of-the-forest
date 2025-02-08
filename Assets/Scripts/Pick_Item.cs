using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pick_Item : MonoBehaviour
{
    private bool playerInRange = false;
    public DialogueManager dialogueManager;
    private BoxCollider2D Collider;
    public int itemID;

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
            dialogueManager.StartDialog();
            Inventory.Instance.AddItem(itemID);
            Destroy(gameObject);
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
