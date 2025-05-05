using System.Collections;
using UnityEngine;

public class Cows : MonoBehaviour
{
    public static Cows Instance { get; private set; }
    Animator animator;
    public bool playerInRange;
    [SerializeField] Pick_Item Pick_Item;
    [SerializeField] TextAsset inkJSON;
    PolygonCollider2D Collider;

    private void Awake()
    {
        Instance = this;
        Collider = GetComponent<PolygonCollider2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && PlayerPrefs.GetInt("cows", 0) != 1)
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
