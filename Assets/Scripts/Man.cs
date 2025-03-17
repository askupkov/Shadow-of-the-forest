using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Man : MonoBehaviour
{
    public Transform destination;
    public float moveSpeed = 1f;
    public Animator animator;
    public BoxCollider2D Collider;
    public TextAsset inkJSON;

    private bool playerInRange = false;

    void Start()
    { 
        Collider = GetComponent<BoxCollider2D>();
        if (animator == null)
        {
            animator = GetComponent<Animator>();
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
        animator.SetBool("IsWalking", true);

        while (Vector2.Distance(transform.position, destination.position) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(
            transform.position,
            destination.position,
            moveSpeed * Time.deltaTime
            );

            yield return null;
        }

        animator.SetBool("IsWalking", false);

        DialogueManager.Instance.StartDialog(inkJSON, "man1");
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
