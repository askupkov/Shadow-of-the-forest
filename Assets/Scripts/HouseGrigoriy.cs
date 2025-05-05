using System.Collections;
using UnityEngine;

public class HouseGrigoriy : MonoBehaviour
{
    [SerializeField] PolygonCollider2D Collider;
    [SerializeField] PolygonCollider2D Collider2;
    private bool playerInRange = false;
    [SerializeField] TextAsset inkJSON;


    private void Update()
    {
        if (playerInRange && Player.Instance.lighting == false)
        {
            StartCoroutine(lighting());
        }
        if (Player.Instance.lighting == true)
        {
            Collider.enabled = false;
            Collider2.enabled = false;
        }
    }

    private IEnumerator lighting()
    {
        DialogueManager.Instance.StartDialog(inkJSON, "temno");
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
