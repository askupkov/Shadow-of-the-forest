using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwampItem : MonoBehaviour
{
    [SerializeField] TextAsset inkJSON;
    [SerializeField] string item;
    [SerializeField] GameObject button;
    private bool playerInRange = false;
    Animator animator;
    

    private void Start()
    {
        button.SetActive(false);
        animator = GetComponent<Animator>();
    }


    private void startItem()
    {
        switch(item)
        {
            case "coin":
                Debug.Log("Coin");
                break;

            case "obereg":
                Debug.Log("Obereg");
                break;

            case "stone":
                Debug.Log("Stone");
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            button.SetActive(true);
            DialogueManager.Instance.StartDialog(inkJSON, item);
            animator.SetBool("TriggerItem", true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            button.SetActive(false);
            animator.SetBool("TriggerItem", false);
        }
    }
}
