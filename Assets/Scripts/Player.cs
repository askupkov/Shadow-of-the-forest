using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private float movingSpeed = 10f;
    [SerializeField] private float runSpeed = 20f;
    public CapsuleCollider2D originalCollider; // »сходный коллайдер
    public CapsuleCollider2D ShiftCollider; // Ќовый коллайдер при нажатии Shift
    public CapsuleCollider2D standCollider;
    public GameObject shadow;
    public GameObject newshadow;
    public VectorValue pos;

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

    private void HandleMovent()
    {
        if (Input.GetKey(KeyCode.LeftShift)) 
        {
            speed = runSpeed;
            
        }
        else
        {
            speed = movingSpeed;
            
        }

        

        Vector2 inputVector = GameInput.Instance.GetMovementVector();
        rb.MovePosition(rb.position + inputVector * (speed * Time.fixedDeltaTime));
        //Debug.Log(inputVector);

        if (speed == runSpeed && inputVector.x != 0)
        {
            ShiftCollider.enabled = true; // ≈сли клавиша Shift нажата, активируем новый коллайдер
            originalCollider.enabled = false;
            shadow.SetActive(false);
            newshadow.SetActive(true);
        }
        else if (speed == movingSpeed && inputVector.x != 0)
        {
            originalCollider.enabled = true;
            ShiftCollider.enabled = false; // ≈сли клавиша Shift не нажата, возвращаем оригинальный коллайдер
            shadow.SetActive(true);
            newshadow.SetActive(false);
        }
        else 
        {
            originalCollider.enabled = false;
            ShiftCollider.enabled = false;
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

        //Debug.Log(isWalking);
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
