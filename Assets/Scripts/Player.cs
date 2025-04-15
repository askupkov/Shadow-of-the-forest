using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

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
    private Vector2 inputVector;
    private bool checkAnimation = true;
    public bool stealth = false;
    private int lastHorizontalDirection = 1;

    private NavMeshAgent navMeshAgent;



    public float speed;

    private Rigidbody2D rb;


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
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.enabled = false;
    }

    public void Noice()
    {
        if (Player.Instance.speed == 2 && inputVector != Vector2.zero)
        {
            NoiseManager.Instance.IncreaseNoise(1f * Time.deltaTime);
        }
        else if (Player.Instance.speed == 5 && inputVector != Vector2.zero)
        {
            NoiseManager.Instance.IncreaseNoise(5f * Time.deltaTime);
        }
        else
        {
            NoiseManager.Instance.DecreaseNoise(0.5f * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        if (checkAnimation)
        {
            HandleMovent();
        }
        if (stealth)
        {
            HandleMoventStealth();
            checkAnimation = false;
        }
    }

    public void Candle()
    {
        lighting = !lighting;
    }

    public void StartDie()
    {
        StartCoroutine(Die());
    }
    private IEnumerator Die()
    {
        GameInput.Instance.OnDisable();
        yield return new WaitForSeconds(0.4f);
        checkAnimation = false;
        shadow.SetActive(false);
        newshadow.SetActive(false);
        PlayerVisual.Instance.TriggerDie();
        yield return new WaitForSeconds(4f);
        GameOver.Instance.ShowGameOverScreen();
        GameInput.Instance.OnDisable();
    }


    private void HandleMovent()
    {

        if (isMovingToDestination)
        {
            navMeshAgent.SetDestination(targetDestination.position);
            inputVector = ((Vector2)targetDestination.position - rb.position).normalized;
        }
        else
        {
            inputVector = GameInput.Instance.GetMovementVector();
            speed = Input.GetKey(KeyCode.LeftShift) && lighting == false ? runSpeed : movingSpeed;
            rb.MovePosition(rb.position + inputVector * (speed * Time.fixedDeltaTime));
        }


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


    private void HandleMoventStealth()
    {
        inputVector = GameInput.Instance.GetMovementVector();
        speed = movingSpeed;
        rb.MovePosition(rb.position + inputVector * (speed * Time.fixedDeltaTime));
        shiftCollider.enabled = false;
        standCollider.enabled = false;
        if (inputVector.x < 0)
        {
            isWalking = 3;
            lastHorizontalDirection = isWalking;
        }
        else if (inputVector.x > 0)
        {
            isWalking = 1;
            lastHorizontalDirection = isWalking;
        }
        else if (inputVector.y != 0)
        {
            isWalking = lastHorizontalDirection;
        }
        else
        {
            isWalking = 0;
        }
    }

    public void StartToMove(Transform destination)
    {
        StartCoroutine(MoveToDestination(destination));
    }

    private IEnumerator MoveToDestination(Transform destination)
    {
        GameInput.Instance.OnDisable();

        
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        isMovingToDestination = true;

        targetDestination = destination;

        if (navMeshAgent != null)
        {
            navMeshAgent.enabled = true;
            navMeshAgent.SetDestination(destination.position);
            while (navMeshAgent.pathPending || navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
            {
                yield return null;
            }
            navMeshAgent.enabled = false;
        }

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
