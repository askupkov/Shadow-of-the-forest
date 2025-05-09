using UnityEngine;


public class SwampItem : MonoBehaviour
{
    [SerializeField] string item;
    private bool playerInRange = false;
    private BoxCollider2D Collider;

    private void Start()
    {
        Collider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if(playerInRange && Input.GetKeyUp(KeyCode.E))
        {
            Swamp.Instance.startItem(item);
            Collider.enabled = false;
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
