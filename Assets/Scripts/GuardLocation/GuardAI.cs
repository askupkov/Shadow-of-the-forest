using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using KnightAdventure.Utils;
using UnityEngine.SceneManagement;



public class GuardAI : MonoBehaviour
{
    public static GuardAI Instance { get; private set; }
    public static List<GuardAI> guards = new List<GuardAI>();
    private NavMeshAgent navMeshAgent;
    private State state;
    private float roamingTimer;
    [SerializeField] private float roamingTimerMax = 2f;
    Animator animator;
    [SerializeField] Vector2 destination1;
    [SerializeField] Vector2 destination2;
    [SerializeField] GameObject Right;
    [SerializeField] GameObject Left;
    [SerializeField] GameObject Back;
    [SerializeField] GameObject Front;

    private bool gameover;
    private bool isWaiting = false;
    private float waitTimer = 0f;
    private Vector2 currentDestination;

    private Rigidbody2D rb;
    private int isWalking;
    private bool playerInRange;


    private enum State
    {
        Roaming,
        Attacking
    }

    private void Awake()
    {
        Instance = this;
        guards.Add(this);
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;

        currentDestination = destination1;
        navMeshAgent.SetDestination(currentDestination);
    }

    private void Update()
    {
        if (!gameover)
        {
            StateHandler();
        }
        HandleMovement();
    }

    private void TriggerDamage()
    {
        PlayerVisual.Instance.TriggerDamage();
        Healthbar.Instance.TakeDamage(100);
    }

    private void StateHandler()
    {
        switch (state)
        {
            default:
            case State.Roaming:
                if (isWaiting)
                {
                    waitTimer -= Time.deltaTime;

                    if (waitTimer <= 0)
                    {
                        isWaiting = false;
                    }
                }
                else
                {
                    roamingTimer -= Time.deltaTime;

                    if (roamingTimer < 0)
                    {
                        Roaming();
                        roamingTimer = roamingTimerMax;
                    }

                    if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
                    {
                        isWaiting = true;
                    }
                }
                CheckCurrentState();
                break;


            case State.Attacking:
                Attacking();
                break;
        }
    }

    public void atack()
    {
        if (IsClosestGuard())
        {
            state = State.Attacking;
        }
    }

    private void Roaming()
    {
        if (!isWaiting)
        {
            currentDestination = (currentDestination == destination1) ? destination2 : destination1;

            navMeshAgent.SetDestination(currentDestination);

            waitTimer = 2f;

            isWaiting = true;
        }
    }

    private void Attacking()
    {
        Vector2 playerPosition = Player.Instance.transform.position;

        float directionX = Mathf.Sign(playerPosition.x - transform.position.x);
        float directionY = Mathf.Sign(playerPosition.y - transform.position.y);

        if (Mathf.Abs(playerPosition.x - transform.position.x) > Mathf.Abs(playerPosition.y - transform.position.y))
        {
            currentDestination = new Vector2(
                playerPosition.x - directionX * 1f,
                playerPosition.y - 0.1f
            );
        }
        else
        {
            currentDestination = new Vector2(
                playerPosition.x,
                playerPosition.y - directionY * 1f
            );
        }

        navMeshAgent.SetDestination(currentDestination);


        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            // Начинаем атаку
            gameover = true;
            animator.SetTrigger("Attack");

            NoiseManager.Instance.GameOver();
        }
    }

    private void CheckCurrentState()
    {
        State newState = State.Roaming;

        if (playerInRange && BushManager.Instance.PlayerHidden == false)
        {
            if (IsClosestGuard())
            {
                GameInput.Instance.OnDisable();
                newState = State.Attacking;
            }
        }
        if (newState != state)
        {
            navMeshAgent.ResetPath();
            state = newState;
        }
    }

    private void HandleMovement()
    {
        Vector2 inputVector;


        inputVector = ((Vector2)currentDestination - rb.position).normalized;


        if (Mathf.Abs(inputVector.x) > Mathf.Abs(inputVector.y))
        {
            if (inputVector.x < 0)
            {
                isWalking = 3;
                Right.SetActive(false);
                Back.SetActive(false);
                Front.SetActive(false);
                Left.SetActive(true);
            }
            else if (inputVector.x > 0)
            {
                isWalking = 1;
                Right.SetActive(true);
                Back.SetActive(false);
                Front.SetActive(false);
                Left.SetActive(false);
            }
        }
        else
        {
            if (inputVector.y > 0)
            {
                isWalking = 4;
                Right.SetActive(false);
                Back.SetActive(true);
                Front.SetActive(false);
                Left.SetActive(false);
            }
            else if (inputVector.y < 0)
            {
                isWalking = 2;
                Right.SetActive(false);
                Back.SetActive(false);
                Front.SetActive(true);
                Left.SetActive(false);
            }
        }

        if (inputVector == Vector2.zero)
        {
            isWalking = 0;
        }

        UpdateAnimations();
    }

    private bool IsClosestGuard()
    {
        float closestDistance = float.MaxValue;
        GuardAI closestGuard = null;

        foreach (var guard in guards)
        {
            float distance = Vector3.Distance(guard.transform.position, Player.Instance.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestGuard = guard;
            }
        }
        return closestGuard == this;
    }

    private void UpdateAnimations()
    {
        animator.SetInteger("IsWalking", isWalking);
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

