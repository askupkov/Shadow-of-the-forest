using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using KnightAdventure.Utils;

public class GuardAI : MonoBehaviour
{
    public static GuardAI Instance { get; private set; }
    public float detectionRadius;
    private NavMeshAgent navMeshAgent;
    private State state;
    private float roamingTimer;
    [SerializeField] private float roamingTimerMax = 2f; // ������������ ����� ��������
    Animator animator;
    [SerializeField] Vector2 destination1;
    [SerializeField] Vector2 destination2;

    private bool gameover;
    private bool isWaiting = false; // ����, ������������, ��������� �� �������� � ��������� ��������
    private float waitTimer = 0f; // ������ ��� ������������ ������� ��������
    private Vector2 currentDestination; // ������� ���� ��������

    private Rigidbody2D rb;
    private int isWalking;

    private enum State
    {
        Roaming,
        Attacking
    }

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;

        // ������������� ��������� ����
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
            case State.Roaming: // ��������
                if (isWaiting)
                {
                    // ��������� ������ ��������
                    waitTimer -= Time.deltaTime;

                    if (waitTimer <= 0)
                    {
                        isWaiting = false; // ��������� ��������
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

                    // ���������, ������ �� �������� ����� ����
                    if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
                    {
                        isWaiting = true; // ���������� �������� �� �����
                    }
                }
                CheckCurrentState();
                break;


            case State.Attacking: // �����
                Attacking();
                break;
        }
    }

    public void atack()
    {
        state = State.Attacking;
    }

    private void Roaming()
    {
        if (!isWaiting) // ���� �������� �� ��������� � ��������� ��������
        {
            // ����������� ������� ����
            currentDestination = (currentDestination == destination1) ? destination2 : destination1;

            // ������������� ����� ���� ��� NavMeshAgent
            navMeshAgent.SetDestination(currentDestination);

            // ��������� ������� ��������
            waitTimer = 2f; // ����� �������� �� �����

            isWaiting = true; // ������������� ���� ��������
        }
    }

    private void Attacking()
    {

        // �������� ������� ������
        Vector2 playerPosition = Player.Instance.transform.position;

        // ���������� ����������� �������� ���� ������������ ������
        float directionX = Mathf.Sign(playerPosition.x - transform.position.x); // ����������� �� X
        float directionY = Mathf.Sign(playerPosition.y - transform.position.y); // ����������� �� Y

        // ��������� ������� ��� �����
        if (Mathf.Abs(playerPosition.x - transform.position.x) > Mathf.Abs(playerPosition.y - transform.position.y))
        {
            // ��� ������������ �� ��� X (�������������)
            currentDestination = new Vector2(
                playerPosition.x - directionX * 1f, // ������ �� X
                playerPosition.y
            );
        }
        else
        {
            // ��� ������������ �� ��� Y (�����������)
            currentDestination = new Vector2(
                playerPosition.x, // ������������ �� X
                playerPosition.y - directionY * 1f
            );
        }

        // ������������� ���� ��� NavMeshAgent
        navMeshAgent.SetDestination(currentDestination);


        // ���������, ������ �� ��� ������� ��� �����
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            // �������� �����
            gameover = true;
            //animator.SetInteger("IsWalking", 5);
            animator.SetTrigger("Attack");

            NoiseManager.Instance.GameOver();
        }
    }

    private void CheckCurrentState() // �������� ���������
    {
        Vector2 raisedPosition = new Vector2(transform.position.x, transform.position.y + 1);
        float distanceToPlayer = Vector3.Distance(raisedPosition, Player.Instance.transform.position);
        State newState = State.Roaming;


        // ���������, ��������� �� ��� �� ������� ��� �����
        if (distanceToPlayer <= detectionRadius && BushManager.Instance.PlayerHidden == false)
        {
            GameInput.Instance.OnDisable();
            newState = State.Attacking;
        }
        if (newState != state)
        {
            navMeshAgent.ResetPath(); // ������������� ��������
            state = newState;
        }
    }

    private void HandleMovement()
    {
        Vector2 inputVector;


        inputVector = ((Vector2)currentDestination - rb.position).normalized;


        if (Mathf.Abs(inputVector.x) > Mathf.Abs(inputVector.y))
        {
            // �������������� �������� ����������
            if (inputVector.x < 0)
            {
                isWalking = 3; // �������� �����
            }
            else if (inputVector.x > 0)
            {
                isWalking = 1; // �������� ������
            }
        }
        else
        {
            // ������������ �������� ����������
            if (inputVector.y > 0)
            {
                isWalking = 4; // �������� �����
            }
            else if (inputVector.y < 0)
            {
                isWalking = 2; // �������� ����
            }
        }

        // ���� ������ �������� ����� ����, �������� �����
        if (inputVector == Vector2.zero)
        {
            isWalking = 0; // ����� �� �����
        }

        UpdateAnimations();
    }
    private void UpdateAnimations()
    {
        animator.SetInteger("IsWalking", isWalking);
    }
}

