using System.Collections;
using UnityEngine;

public class Pick_Item : MonoBehaviour
{
    public static Pick_Item Instance { get; private set; }
    [SerializeField] TextAsset inkJSON;
    public string startingPoint;
    private bool playerInRange = false;
    private BoxCollider2D Collider;
    public int itemID;
    public GameObject item;
    public bool DestroyItem = false;
    private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
        Collider = GetComponent<BoxCollider2D>();
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        AudioSetting.Instance.RegisterSfx(audioSource);
        if (PlayerPrefs.GetInt(gameObject.name, 0) == 1 && DestroyItem)
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !Pause.Instance.pauseOpen)
        {
            StartCoroutine(pick_item());
        }
    }

    private IEnumerator pick_item()
    {
        audioSource.Play();
        Collider.enabled = false;
        GameInput.Instance.OnDisable();
        Item selectedItem = Inventory.Instance.data.items[itemID];
        InspectItem.Instance.ShowItem(selectedItem.img_insp);

        yield return new WaitForSeconds(0.1f);
        while (!Input.GetKeyDown(KeyCode.E))
        {
            yield return null;
        }
        InspectItem.Instance.HideItem();
        DialogueManager.Instance.StartDialog(inkJSON, startingPoint);
        Inventory.Instance.AddItem(itemID);
        if (DestroyItem == true)
        {
            PickUpItem();
        }
    }

    public void PickUpItem()
    {
        ItemController.Instance.addPickedItems(gameObject.name);
        Destroy(gameObject);
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
