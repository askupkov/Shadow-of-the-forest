using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dialog_starushka : MonoBehaviour
{
    private Animator Animator;
    private bool playerInRange = false;
    private bool isSecondDialogStarted = false; // Флаг для второго диалога
    private bool isThirdDialogStarted = false;  // Флаг для третьего диалога
    public TextAsset inkJSON;
    public BoxCollider2D Collider;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Collider = GetComponent<BoxCollider2D>();
        StartCoroutine(StartInitialDialog());
    }

    private IEnumerator StartInitialDialog()
    {
        //yield return new WaitForSeconds(0.3f);
        DialogueManager.Instance.StartDialog(inkJSON, "starushka0");
        while (DialogueManager.Instance.dialogPanelOpen)
        {
            yield return null;
        }
        DialogueManager.Instance.StartDialog(inkJSON, "starushka1");
        isSecondDialogStarted = true;
        Animator.SetBool("Dialog_starushka", true);
    }

    private void Update()
    {
        if (playerInRange && isSecondDialogStarted)
        {
            DialogueManager.Instance.StartDialog(inkJSON, "starushka2");
            Collider.enabled = false;
            isThirdDialogStarted = true;
        }

        if (!DialogueManager.Instance.dialogPanelOpen && isThirdDialogStarted)
        {
            Animator.SetBool("Dialog_starushka", false);
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