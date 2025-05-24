using System.Collections;
using UnityEngine;

public class Cows : MonoBehaviour
{
    public static Cows Instance { get; private set; }
    public bool playerInRange;
    [SerializeField] TextAsset inkJSON;
    PolygonCollider2D Collider;
    private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
        Collider = GetComponent<PolygonCollider2D>();
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        AudioSetting.Instance.RegisterSfx(audioSource);
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && PlayerPrefs.GetInt("cows", 0) != 1 && PlayerPrefs.GetInt("firstDialogueDomovoy", 0) == 1)
        {
            DialogueManager.Instance.StartDialog(inkJSON, "cows");
        }
        else if (playerInRange && Input.GetKeyDown(KeyCode.E) && PlayerPrefs.GetInt("cows", 0) != 1)
        {
            DialogueManager.Instance.StartDialog(inkJSON, "cows2");
        }
        StartCoroutine(cows());
    }

    private IEnumerator cows()
    {
        while (DialogueManager.Instance.dialogPanelOpen)
        {
            Collider.enabled = false;
            yield return null;
        }
        Collider.enabled = true;
    }

    public void bucket()
    {
        if (playerInRange)
        {
            Inventory.Instance.ConsumeItem(8);
            Inventory.Instance.AddItem(9);
            PlayerPrefs.SetInt("cows", 1);
            audioSource.Play();
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
