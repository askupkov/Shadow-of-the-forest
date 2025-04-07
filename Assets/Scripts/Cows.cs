using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cows : MonoBehaviour
{
    Animator animator;
    private bool playerInRange;
    [SerializeField] Pick_Item Pick_Item;
    [SerializeField] TextAsset inkJSON;
    BoxCollider2D Collider;

    private void Awake()
    {
        Collider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if(Inventory.Instance.HasItem(8) == true)
            {
                Inventory.Instance.ConsumeItem(8);
                Pick_Item.pick_item();
            }
            else
            {
                DialogueManager.Instance.StartDialog(inkJSON, "cows");
            }
            if(DialogueManager.Instance.dialogPanelOpen == true)
            {
                Collider.enabled = false;
            }
            else
            {
                Collider.enabled = true;
            }
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
