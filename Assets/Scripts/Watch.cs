using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watch : MonoBehaviour
{
    public TextAsset inkJSON;
    public string startingPoint;
    private bool playerInRange = false;
    private DialogueManager dialogueManager;
    private BoxCollider2D Collider;

    private void Start()
    {
        Collider = GetComponent<BoxCollider2D>();
        dialogueManager = FindObjectOfType<DialogueManager>();
    }
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            dialogueManager.StartDialog(inkJSON, startingPoint);

        }
        if (dialogueManager.dialogPanelOpen == true)
        {
            Collider.enabled = false;
        }
        else
        {
            Collider.enabled = true;
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
