using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dialog_starushka : MonoBehaviour
{
    private Animator Animator;
    private bool playerInColliderRange = false;
    private bool playerInCollider2Range = false;
    private bool isSecondDialogStarted = false; // Флаг для второго диалога
    private bool isThirdDialogStarted = false;  // Флаг для третьего диалога
    [SerializeField] TextAsset inkJSON;
    private BoxCollider2D Collider;
    [SerializeField] BoxCollider2D Collider2;

    private string PlayerPrefs => $"{gameObject.name}";

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Collider = GetComponent<BoxCollider2D>();
        LoadState();
    }

    private IEnumerator StartInitialDialog()
    {
        GameInput.Instance.OnDisable();
        yield return new WaitForSeconds(0.3f);
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
        if (playerInColliderRange && isSecondDialogStarted)
        {
            DialogueManager.Instance.StartDialog(inkJSON, "starushka2");
            Collider.enabled = false;
            playerInColliderRange = false;
            isThirdDialogStarted = true;
        }

        if (playerInCollider2Range)
        {
            StartCoroutine(StartInitialDialog());
            Collider2.enabled = false;
            playerInCollider2Range = false;
            UnityEngine.PlayerPrefs.SetInt($"{PlayerPrefs}_Collider2Enabled", 0);
            UnityEngine.PlayerPrefs.Save();
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
            if (other.IsTouching(Collider))
            {
                playerInColliderRange = true;
            }
            else if (other.IsTouching(Collider2))
            {
                playerInCollider2Range = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.IsTouching(Collider))
            {
                playerInColliderRange = false;
            }
            else if (other.IsTouching(Collider2))
            {
                playerInCollider2Range = false;
            }
        }
    }

    private void LoadState()
    {
        if (UnityEngine.PlayerPrefs.HasKey($"{PlayerPrefs}_Collider2Enabled"))
        {
            Collider2.enabled = UnityEngine.PlayerPrefs.GetInt($"{PlayerPrefs}_Collider2Enabled") == 1;
        }
        else
        {
            UnityEngine.PlayerPrefs.SetInt(PlayerPrefs, Collider2.enabled ? 1 : 0);
            UnityEngine.PlayerPrefs.Save();
        }
    }
}