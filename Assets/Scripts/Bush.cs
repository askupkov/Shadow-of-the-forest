using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{
    private bool playerInRange = false;
    private Animator Animator;
    private BoxCollider2D Collider;
    private string PlayerPrefsKey => $"{gameObject.name}";

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        Collider = GetComponent<BoxCollider2D>();
    }
    private void Start()
    {
        bool hasBerries = PlayerPrefs.GetInt(PlayerPrefsKey, 0) == 1;
        Animator.SetBool("With_berries", true);

        if (hasBerries)
        {
            Animator.SetBool("With_berries", false);
            Collider.enabled = false;
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            CollectBerries();
        }
    }

    private void CollectBerries()
    {
        if (Animator.GetBool("With_berries"))
        {
            Animator.SetBool("With_berries", false);

            ItemController.Instance.addPickedItems(gameObject.name);

            Collider.enabled = false;

            Inventory.Instance.AddItem(3);
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