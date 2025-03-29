using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    private bool playerInRange = false;
    private BoxCollider2D Collider;
    [SerializeField] Transform destination1;
    [SerializeField] Transform destination2;
    [SerializeField] Transform destination3;
    [SerializeField] GameObject mouse;
    [SerializeField] TextAsset inkJSON;
    [SerializeField] Animator animator;

    private void Start()
    {
        Collider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (playerInRange)
        {
            Collider.enabled = false;
            StartCoroutine(mouse_run());
            playerInRange = false;
        }
    }
    IEnumerator mouse_run()
    {
        GameInput.Instance.OnDisable();
        yield return new WaitForSeconds(1f);
        NPC.Instance.StartToMove(destination1);
        while (NPC.Instance.isMovingToDestination)
        {
            yield return null;
        }
        NPC.Instance.StartToMove(destination2);
        while (NPC.Instance.isMovingToDestination)
        {
            yield return null;
        }
        NPC.Instance.StartToMove(destination3);
        while (NPC.Instance.isMovingToDestination)
        {
            yield return null;
        }
        Destroy(mouse);
        DialogueManager.Instance.StartDialog(inkJSON, "house_grigoriy");
        while (DialogueManager.Instance.dialogPanelOpen)
        {
            yield return null;
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
