using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindmillFirstFloor : MonoBehaviour
{
    private bool playerInRange = false;
    [SerializeField] TextAsset inkJSON;
    [SerializeField] SceneController sceneController;
    private bool second;

    private void Start()
    {
        if (PlayerPrefs.GetInt(gameObject.name, 0) == 1)
        {
            second = true;
        }
    }

    private void Update()
    {
        if (playerInRange && !second)
        {
            sceneController.StartLoadScene(15);
            PlayerPrefs.SetInt(gameObject.name, 1);
        }
        else if (playerInRange && second)
        {
            {
                if (Inventory.Instance.HasItem(7) == true && Inventory.Instance.HasItem(9) == true)
                {
                    sceneController.StartLoadScene(15);
                }
                else
                {
                    DialogueManager.Instance.StartDialog(inkJSON, "domovoy2");
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
