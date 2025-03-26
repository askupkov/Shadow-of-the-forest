using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volhv : MonoBehaviour
{
    public static Volhv Instance { get; private set; }
    private Animator animator;
    private float speed = 0.7f;
    private Rigidbody2D rb;
    private Transform targetDestination;
    public bool isMovingToDestination;

    private int isWalking;

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 inputVector;
        if (isMovingToDestination)
        {
            inputVector = ((Vector2)targetDestination.position - rb.position).normalized;
            rb.MovePosition(rb.position + inputVector * (speed * Time.fixedDeltaTime));
        }
        else
        {
            inputVector.x = 0f;
            inputVector.y = 0f;
        }
       
        if (inputVector.x < 0)
        {
            isWalking = 3;
        }
        else if (inputVector.x > 0)
        {
            isWalking = 1;
        }
        else if (inputVector.y > 0)
        {
            isWalking = 4;
        }
        else if (inputVector.y < 0)
        {
            isWalking = 2;
        }
        else
        {
            isWalking = 0;
        }
        UpdateAnimations();
    }

    public void StartToMove(Transform destination)
    {
        StartCoroutine(MoveToDestination(destination));
    }

    private IEnumerator MoveToDestination(Transform destination)
    {
        isMovingToDestination = true;
        targetDestination = destination;

        while (Vector2.Distance(transform.position, destination.position) > 0.1f)
        {
            yield return null;
        }
        transform.position = destination.position;
        isMovingToDestination = false;
    }
    private void UpdateAnimations()
    {
        animator.SetInteger("IsWalking", isWalking);
    }
}