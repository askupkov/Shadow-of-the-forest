using System.Collections;
using UnityEngine;

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
    [SerializeField] Transform destination;


    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Collider = GetComponent<BoxCollider2D>();
        LoadState();
        if(PlayerPrefs.GetInt("startVilage", 0) != 1)
        {
            DialogueManager.Instance.StartDialog(inkJSON, "starushka0");
        }
        PlayerPrefs.SetInt("startVilage", 1);
    }

    private IEnumerator StartInitialDialog()
    {
        Player.Instance.StartToMove(destination);
        while (Player.Instance.isMovingToDestination)
        {
            yield return null;
        }
        GameInput.Instance.OnDisable();
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
            PlayerPrefs.SetInt("Dialog_starushka", 1);
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
        if (PlayerPrefs.GetInt("Dialog_starushka", 0) == 1)
        {
            Collider2.enabled = false;
        }
    }
}