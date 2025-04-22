using UnityEngine;

public class WindmillFirstFloor : MonoBehaviour
{
    private bool playerInRange = false;
    [SerializeField] TextAsset inkJSON;
    [SerializeField] SceneController sceneController;
    private bool second;
    private bool final;

    private void Start()
    {
        if (PlayerPrefs.GetInt(gameObject.name, 0) == 1)
        {
            second = true;
        }
        if (PlayerPrefs.GetInt("finaldomovoy", 0) == 1)
        {
            final = true;
        }
    }

    private void Update()
    {
        if (playerInRange && !second && !final)
        {
            sceneController.StartLoadScene(15);
            PlayerPrefs.SetInt(gameObject.name, 1);
        }
        else if (playerInRange && second && !final)
        {
            {
                if (Inventory.Instance.HasItem(7) == true && Inventory.Instance.HasItem(9) == true)
                {
                    sceneController.StartLoadScene(15);
                    PlayerPrefs.SetInt("finaldomovoy", 1);
                }
                else
                {
                    DialogueManager.Instance.StartDialog(inkJSON, "domovoy4");
                    playerInRange = false;
                }
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
