using UnityEngine;

public class LoadSceneHouseGG : MonoBehaviour
{
    [SerializeField] SceneController sceneController;
    private bool playerInRange;
    [SerializeField] TextAsset inkJSON;

    void Update()
    {
        if (playerInRange)
        {
            if (Inventory.Instance.HasItem(1) == true && Inventory.Instance.HasItem(12) == true)
            {
                sceneController.StartLoadScene(4);
            }
            else
            {
                DialogueManager.Instance.StartDialog(inkJSON, "house");
                playerInRange = false;
            }
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
