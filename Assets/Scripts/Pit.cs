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
    [SerializeField] Animator playerAnim;
    [SerializeField] Transform destination1;
    [SerializeField] Transform destination2;
    [SerializeField] Transform destination3;
    [SerializeField] BoxCollider2D Collider;
    [SerializeField] BoxCollider2D Collider2;

    private void Awake()
    {
        Instance = this;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (playerInColliderRange)
        {
            Collider.enabled = false;
            StartCoroutine(MoveToPit());
        }
        if (playerInCollider2Range && withrope && Input.GetKeyDown(KeyCode.E))
        {
            SceneController.Instance.StartLoadScene(12);
        }
    }

    public void Withrope()
    {
        animator.SetTrigger("Rope");
        withrope = true;
    }

    private IEnumerator MoveToPit()
    {
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
        Player.Instance.StartToMove(destination3);
        while (Player.Instance.isMovingToDestination)
        {
            yield return null;
        }
        playerAnim.SetBool("LookPit", true);
        yield return new WaitForSeconds(2f);
        playerAnim.SetBool("LookPit", false);
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
    
