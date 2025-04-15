using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pit : MonoBehaviour
{
    public static Pit Instance { get; private set; }
    Animator animator;
    [SerializeField] SceneController sceneController;
    private bool playerInColliderRange;
    public bool playerInCollider2Range;
    private bool withrope;
    [SerializeField] TextAsset inkJSON;
    [SerializeField] Animator playerAnim;
    [SerializeField] Transform destination1;
    [SerializeField] Transform destination2;
    [SerializeField] Transform destination3;
    [SerializeField] BoxCollider2D Collider;
    [SerializeField] BoxCollider2D Collider2;
    [SerializeField] GameObject womens;
    private bool visitpit = false;

    private void Awake()
    {
        Instance = this;
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt(gameObject.name, 0) == 1)
        {
            animator.SetTrigger("Rope");
            withrope = true;
        }
        if(PlayerPrefs.GetInt("ColliderPit", 0)== 1)
        {
            Collider.enabled = false;
            Destroy(womens);
        }
        if (PlayerPrefs.GetInt("VisitPit", 0) == 1)
        {
            visitpit = true;
        }
    }   

    private void Update()
    {
        if (playerInColliderRange)
        {
            Collider.enabled = false;
            PlayerPrefs.SetInt("ColliderPit", 1);
            PlayerPrefs.Save();
            StartCoroutine(MoveToPit());
        }
        if (playerInCollider2Range && withrope && Input.GetKeyDown(KeyCode.E))
        {
            if (visitpit == false)
            {
                SceneController.Instance.StartLoadScene(12);
                visitpit = true;
                PlayerPrefs.SetInt("VisitPit", 1);
                PlayerPrefs.Save();
            }
            else
            {
                if (DialogueManager.Instance.dialogPanelOpen == true)
                {
                    Collider2.enabled = false;
                }
                else
                {
                    Collider2.enabled = true;
                }
                DialogueManager.Instance.StartDialog(inkJSON, "pit");
            }
        }
    }

    public void Withrope()
    {
        animator.SetTrigger("Rope");
        withrope = true;
        PlayerPrefs.SetInt(gameObject.name, 1);
        PlayerPrefs.Save();
    }

    private IEnumerator MoveToPit()
    {
        Player.Instance.StartToMove(destination1);
        while (Player.Instance.isMovingToDestination)
        {
            yield return null;
        }
        DialogueManager.Instance.StartDialog(inkJSON, "pit1");
        while (DialogueManager.Instance.dialogPanelOpen)
        {
            yield return null;
        }
        Player.Instance.StartToMove(destination2);
        while (Player.Instance.isMovingToDestination)
        {
            yield return null;
        }
        GameInput.Instance.OnDisable();
        playerAnim.SetBool("LookPit", true);
        yield return new WaitForSeconds(2f);
        playerAnim.SetBool("LookPit", false);
        DialogueManager.Instance.StartDialog(inkJSON, "pit2");
        while (DialogueManager.Instance.dialogPanelOpen)
        {
            yield return null;
        }
        DialogueManager.Instance.StartDialog(inkJSON, "pit3");
        while (DialogueManager.Instance.dialogPanelOpen)
        {
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
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
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!other.IsTouching(Collider))
            {
                playerInColliderRange = false;
            }
            if (!other.IsTouching(Collider2))
            {
                playerInCollider2Range = false;
            }
        }
    }
}
    
