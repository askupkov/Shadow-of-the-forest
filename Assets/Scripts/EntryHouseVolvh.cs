using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryHouseVolvh : MonoBehaviour
{
    private bool playerInRange;
    [SerializeField] Transform destination;
    [SerializeField] Animator animator;
    [SerializeField] SceneController sceneController;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && PlayerPrefs.GetInt("loadHouseVolvh") == 1)
        {
            StartCoroutine(loadHouseVolvh());
        }
    }

    private IEnumerator loadHouseVolvh()
    {
        Player.Instance.StartToMove(destination);
        while (Player.Instance.isMovingToDestination)
        {
            yield return null;
        }
        GameInput.Instance.OnDisable();
        animator.SetTrigger("EnterDoor");
        yield return new WaitForSeconds(0.6f);
        sceneController.StartLoadScene(8);
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
