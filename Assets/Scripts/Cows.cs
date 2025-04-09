using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cows : MonoBehaviour
{
    public static Cows Instance { get; private set; }
    Animator animator;
    public bool playerInRange;
    [SerializeField] Pick_Item Pick_Item;
    [SerializeField] TextAsset inkJSON;
    BoxCollider2D Collider;

    private void Awake()
    {
        Instance = this;
        Collider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(cows());
        }
    }

    private IEnumerator cows()
    {
        DialogueManager.Instance.StartDialog(inkJSON, "cows");
        while (DialogueManager.Instance.dialogPanelOpen)
        {
            Collider.enabled = false;
            yield return null;
        }
        Collider.enabled = true;
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
