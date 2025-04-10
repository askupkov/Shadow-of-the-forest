using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private float movingSpeed = 10f;
    [SerializeField] private float runSpeed = 20f;
    public CapsuleCollider2D originalCollider; // Исходный коллайдер
    public CapsuleCollider2D shiftCollider; // Новый коллайдер при нажатии Shift
    public CapsuleCollider2D standCollider;
    public GameObject shadow;
    public GameObject newshadow;
    public VectorValue pos;
    public bool isMovingToDestination = false;
    private Transform targetDestination;
    public bool lighting;
    public bool damage;

    private float speed;

    private Rigidbody2D rb;

    //private float minMovingSpeed = 0.1f;
    private int isWalking = 0;
    private int isRunning = 0;

    private void Start()
    {
        transform.position = pos.initialValue;
    }

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();

    }

    private void FixedUpdate()
    {
        HandleMovent();
    }

    public void Candle()
    {
        lighting = !lighting;
    }


    private void HandleMovent()
    {
        Vector2 inputVector;
        if (isMovingToDestination)
        {
            inputVector = ((Vector2)targetDestination.position - rb.position).normalized;
            speed = movingSpeed;
        }
        else
        {
            inputVector = GameInput.Instance.GetMovementVector();
            speed = Input.GetKey(KeyCode.LeftShift) && lighting == false ? runSpeed : movingSpeed;
        }
        rb.MovePosition(rb.position + inputVector * (speed * Time.fixedDeltaTime));

        //Debug.Log(inputVector);

        if (speed == runSpeed && inputVector.x != 0)
        {
            shiftCollider.enabled = true;
            originalCollider.enabled = true;
            standCollider.enabled = true;
            shadow.SetActive(false);
            newshadow.SetActive(true);
        }
        else if (speed == movingSpeed && inputVector.x != 0)
        {
            originalCollider.enabled = true;
            shiftCollider.enabled = false;
            standCollider.enabled = false;
            shadow.SetActive(true);
            newshadow.SetActive(false);
        }
        else 
        {
            standCollider.enabled = true;
            originalCollider.enabled = false;
            shiftCollider.enabled = false;
            shadow.SetActive(true);
            newshadow.SetActive(false);
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

        if (inputVector.x < 0 && speed == runSpeed)
        {
            isRunning = 3;
        }
        else if (inputVector.x > 0 && speed == runSpeed)
        {
            isRunning = 1;
        }
        else if (inputVector.y > 0 && speed == runSpeed)
        {
            isRunning = 4;
        }
        else if (inputVector.y < 0 && speed == runSpeed)
        {
            isRunning = 2;
        }
        else
        {
            isRunning = 0;
        }
    }

    public void StartToMove(Transform destination)
    {
        StartCoroutine(MoveToDestination(destination));
    }

    private IEnumerator MoveToDestination(Transform destination)
    {
        GameInput.Instance.OnDisable();
        isMovingToDestination = true;
        targetDestination = destination;

        while (Vector2.Distance(transform.position, destination.position) > 0.1f)
        { 
            yield return null;
        }
        transform.position = destination.position;
        isMovingToDestination = false;
        GameInput.Instance.OnEnabled();
    }


    public bool IsLighting()
    {
        return lighting;
    }
    public int IsWalking()
    {
        return isWalking;
    }
    public int IsRunning()
    {
        return isRunning;
    }

}
