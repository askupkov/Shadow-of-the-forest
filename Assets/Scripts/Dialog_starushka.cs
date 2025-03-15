using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dialog_starushka : MonoBehaviour
{
    private Animator Animator;
    private bool playerInRange = false;
    private bool dialog = false;
    public TextAsset inkJSON;
    public BoxCollider2D Collider;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }
    private void Start()
    {
        Collider = GetComponent<BoxCollider2D>();
        StartCoroutine(start_dialog());
    }

    void Update()
    {
        if (playerInRange)
        {
            DialogueManager.Instance.StartDialog(inkJSON, "starushka2");
            Collider.enabled = false;
        }
        if (DialogueManager.Instance.dialogPanelOpen == true)
        {
            
        }
        else
        {
            dialog = false;
            Animator.SetBool("Dialog_starushka", dialog);
        }
    }

    private IEnumerator start_dialog()
    {
        yield return new WaitForSeconds(0.4f);
        dialog = true;
        Animator.SetBool("Dialog_starushka", dialog);
        DialogueManager.Instance.StartDialog(inkJSON, "starushka1");
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