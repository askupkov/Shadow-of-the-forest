using System.Collections;
using UnityEngine;

public class HouseGrigoriy : MonoBehaviour
{
    [SerializeField] PolygonCollider2D Collider;
    [SerializeField] PolygonCollider2D Collider2;
    private bool playerInRange = false;
    [SerializeField] TextAsset inkJSON;


    private void Start()
    {
        if (PlayerPrefs.GetInt("HouseGrigoriy", 0) == 1)
        {
            Collider.enabled = false;
            Collider2.enabled = false;
        }
    }

    private void Update()
    {
        if (playerInRange && Player.Instance.lighting == false)
        {
            DialogueManager.Instance.StartDialog(inkJSON, "temno");
            playerInRange = false;
        }
        if (Player.Instance.lighting == true)
        {
            Collider.enabled = false;
            Collider2.enabled = false;
            PlayerPrefs.SetInt("HouseGrigoriy", 1);
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
