using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Man : MonoBehaviour
{
    [SerializeField] Transform destination1;
    [SerializeField] Transform destination2;
    [SerializeField] Transform destination3;
    public float moveSpeed = 1f;
    public Animator animator;
    public Animator animator2;
    public BoxCollider2D Collider;
    public TextAsset inkJSON;

    private bool playerInRange = false;

    void Start()
    { 
        Collider = GetComponent<BoxCollider2D>();
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            animator2 = GetComponent<Animator>();
        }
    }

    private void Update()
    {
        if (playerInRange)
        {
            Collider.enabled = false;
            GameInput.Instance.OnDisable();
            StartCoroutine(MoveToDestination());
        }
    }

    private IEnumerator MoveToDestination()
    {
        Player.Instance.StartToMove(destination3);
        while (Player.Instance.isMovingToDestination)
        {
            yield return null;
        }
        GameInput.Instance.OnDisable();
        animator.SetBool("IsWalking", true);
        while (Vector2.Distance(transform.position, destination1.position) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(
            transform.position,
            destination1.position,
            moveSpeed * Time.deltaTime
            );

            yield return null;
        }
        animator.SetBool("Right", true);
        while (Vector2.Distance(transform.position, destination2.position) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(
            transform.position,
            destination2.position,
            moveSpeed * Time.deltaTime
            );

            yield return null;
        }

        animator2.SetBool("Stop", true);
        animator.SetBool("IsWalking", false);

        DialogueManager.Instance.StartDialog(inkJSON, "man1");
        while (DialogueManager.Instance.dialogPanelOpen)
        {
            yield return null;
        }
        GameInput.Instance.OnDisable();
        animator2.SetBool("ComeIn", true);
        yield return new WaitForSeconds(2f);
        DialogueManager.Instance.StartDialog(inkJSON, "man2");
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
