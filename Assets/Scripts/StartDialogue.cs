using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StartDialogue : MonoBehaviour
{
    private bool playerInRange;
    public TextAsset inkJSON;
    public string Tag;
    private Collider2D Collider;

    private void Start()
    {
        Collider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (playerInRange)
        {
            DialogueManager.Instance.StartDialog(inkJSON, Tag);
            Collider.enabled = false;
            playerInRange = false;
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
