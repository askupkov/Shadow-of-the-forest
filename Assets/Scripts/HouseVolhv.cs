using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class HouseVolhv : MonoBehaviour
{
    private bool playerInRange = false;
    public Transform destination1;
    public Transform destination2;
    public Transform destination3;
    private BoxCollider2D Collider;
    public Animator animator; // —сылка на Animator
    public string sceneToLoad;
    public TextAsset inkJSON;
    public GameObject volhv;

    private void Start()
    {
        Collider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (playerInRange)
        {
            Collider.enabled = false;
            StartCoroutine(VolhvDialogue());
        }
    }

    private IEnumerator VolhvDialogue()
    {
        DialogueManager.Instance.StartDialog(inkJSON, "volhv");
        while (DialogueManager.Instance.dialogPanelOpen)
        {
            yield return null;
        }
        GameInput.Instance.OnDisable();
        Volhv.Instance.StartToMove(destination2);
        while (Volhv.Instance.isMovingToDestination)
        {
            yield return null;
        }
        Volhv.Instance.StartToMove(destination3);
        while (Volhv.Instance.isMovingToDestination)
        {
            yield return null;
        }
        Destroy(volhv);
        yield return new WaitForSeconds(1f);
        Player.Instance.StartToMove(destination1);
        while (Player.Instance.isMovingToDestination)
        {
            yield return null;
        }
        Player.Instance.StartToMove(destination2);
        while (Player.Instance.isMovingToDestination)
        {
            yield return null;
        }
        animator.SetTrigger("EnterDoor");
        yield return new WaitForSeconds(0.6f);
        SceneController.Instance.StartLoadScene(sceneToLoad);
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
