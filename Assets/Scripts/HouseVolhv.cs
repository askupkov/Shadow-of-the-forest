using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class HouseVolhv : MonoBehaviour
{
    [SerializeField] SceneController sceneController;
    private bool playerInColliderRange = false;
    private bool playerInCollider2Range = false;
    [SerializeField] Transform destination1;
    [SerializeField] Transform destination2;
    [SerializeField] Transform destination3;
    private BoxCollider2D Collider;
    [SerializeField] Animator animator;
    [SerializeField] int sceneToLoad;
    [SerializeField] TextAsset inkJSON;
    [SerializeField] GameObject volhv;
    [SerializeField] BoxCollider2D Collider2;


    private void Start()
    {
        Collider = GetComponent<BoxCollider2D>();
        LoadState();
        if (PlayerPrefs.GetInt(volhv.name, 0) == 1)
        {
            Destroy(volhv);
        }
    }

    private void Update()
    {
        if (playerInColliderRange)
        {
            Collider.enabled = false;
            PlayerPrefs.SetInt("Volhv_ColliderEnabled", 1);
            PlayerPrefs.Save();
            StartCoroutine(VolhvDialogue());
            playerInColliderRange = false;
        }
        if (playerInCollider2Range)
        {
            Collider2.enabled = false;
            PlayerPrefs.SetInt("Volhv_Collider2Enabled", 0);
            PlayerPrefs.SetInt("loadHouseVolvh", 1);
            PlayerPrefs.Save();
            StartCoroutine(Dialogue());
            playerInCollider2Range = false;
        }
    }

    private IEnumerator VolhvDialogue()
    {
        Player.Instance.StartToMove(destination1);
        while (Player.Instance.isMovingToDestination)
        {
            yield return null;
        }
        DialogueManager.Instance.StartDialog(inkJSON, "volhv");
        while (DialogueManager.Instance.dialogPanelOpen)
        {
            yield return null;
        }
        GameInput.Instance.OnDisable();
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
        Destroy(volhv);
        PlayerPrefs.SetInt(volhv.name, 1);
        PlayerPrefs.Save();
        yield return new WaitForSeconds(1f);
        Player.Instance.StartToMove(destination2);
        while (Player.Instance.isMovingToDestination)
        {
            yield return null;
        }
        GameInput.Instance.OnDisable();
        animator.SetTrigger("EnterDoor");
        yield return new WaitForSeconds(0.6f);
        sceneController.StartLoadScene(sceneToLoad);
        Collider2.enabled = true;
        PlayerPrefs.SetInt("Volhv_Collider2Enabled", 1);
        PlayerPrefs.Save();
    }

    private IEnumerator Dialogue()
    {
        GameInput.Instance.OnDisable();
        DialogueManager.Instance.StartDialog(inkJSON, "volhv3");
        while (DialogueManager.Instance.dialogPanelOpen)
        {
            yield return null;
        }
        GameInput.Instance.OnDisable();
        animator.SetTrigger("Head");
        yield return new WaitForSeconds(1.3f);
        GameInput.Instance.OnEnabled();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.IsTouching(Collider))
        {
            playerInColliderRange = true;
        }
        else if (other.IsTouching(Collider2))
        {
            playerInCollider2Range = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.IsTouching(Collider))
            {
                playerInColliderRange = false;
            }
            else if (other.IsTouching(Collider2))
            {
                playerInCollider2Range = false;
            }
        }
    }

    private void LoadState()
    {
        //if (PlayerPrefs.HasKey("Volhv_ColliderEnabled"))
        //{
        //    Collider.enabled = PlayerPrefs.GetInt("Volhv_ColliderEnabled") == 1;
        //}
        //if (PlayerPrefs.HasKey("Volhv_Collider2Enabled"))
        //{
        //    Collider2.enabled = PlayerPrefs.GetInt("Volhv_Collider2Enabled") == 1;
        //}
        if (PlayerPrefs.GetInt("Volhv_ColliderEnabled", 0) == 1)
        {
            Collider.enabled = false;
        }
        if (PlayerPrefs.GetInt("Volhv_Collider2Enabled", 0) == 1)
        {
            Collider2.enabled = true;
        }
    }
}
