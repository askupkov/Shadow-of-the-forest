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
    [SerializeField] private float roamingTimerMax = 2f; // Максимальное время брожения
    Animator animator;
    [SerializeField] Vector2 destination1;
    [SerializeField] Vector2 destination2;

    private bool gameover;
    private bool isWaiting = false; // Флаг, показывающий, находится ли стражник в состоянии ожидания
    private float waitTimer = 0f; // Таймер для отслеживания времени ожидания
    private Vector2 currentDestination; // Текущая цель движения

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

        // Устанавливаем начальную цель
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
            case State.Roaming: // Брожение
                if (isWaiting)
                {
                    // Уменьшаем таймер ожидания
                    waitTimer -= Time.deltaTime;

                    if (waitTimer <= 0)
                    {
                        isWaiting = false; // Завершаем ожидание
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

                    // Проверяем, достиг ли стражник своей цели
                    if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
                    {
                        isWaiting = true; // Активируем ожидание на точке
                    }
                }
                CheckCurrentState();
                break;


            case State.Attacking: // Атака
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
        if (!isWaiting) // Если стражник не находится в состоянии ожидания
        {
            // Переключаем текущую цель
            currentDestination = (currentDestination == destination1) ? destination2 : destination1;

            // Устанавливаем новую цель для NavMeshAgent
            navMeshAgent.SetDestination(currentDestination);

            // Генерация времени ожидания
            waitTimer = 2f; // Время ожидания на точке

            isWaiting = true; // Устанавливаем флаг ожидания
        }
    }

    private void Attacking()
    {

        // Получаем позицию игрока
        Vector2 playerPosition = Player.Instance.transform.position;

        // Определяем направление движения моба относительно игрока
        float directionX = Mathf.Sign(playerPosition.x - transform.position.x); // Направление по X
        float directionY = Mathf.Sign(playerPosition.y - transform.position.y); // Направление по Y

        // Вычисляем позицию для атаки
        if (Mathf.Abs(playerPosition.x - transform.position.x) > Mathf.Abs(playerPosition.y - transform.position.y))
        {
            // Моб приближается по оси X (горизонтально)
            currentDestination = new Vector2(
                playerPosition.x - directionX * 1f, // Отступ по X
                playerPosition.y
            );
        }
        else
        {
            // Моб приближается по оси Y (вертикально)
            currentDestination = new Vector2(
                playerPosition.x, // Выравнивание по X
                playerPosition.y - directionY * 1f
            );
        }

        // Устанавливаем цель для NavMeshAgent
        navMeshAgent.SetDestination(currentDestination);


        // Проверяем, достиг ли моб позиции для атаки
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            // Начинаем атаку
            gameover = true;
            //animator.SetInteger("IsWalking", 5);
            animator.SetTrigger("Attack");

            NoiseManager.Instance.GameOver();
        }
    }

    private void CheckCurrentState() // Проверка состояния
    {
        Vector2 raisedPosition = new Vector2(transform.position.x, transform.position.y + 1);
        float distanceToPlayer = Vector3.Distance(raisedPosition, Player.Instance.transform.position);
        State newState = State.Roaming;


        // Проверяем, находится ли моб на позиции для атаки
        if (distanceToPlayer <= detectionRadius && BushManager.Instance.PlayerHidden == false)
        {
            GameInput.Instance.OnDisable();
            newState = State.Attacking;
        }
        if (newState != state)
        {
            navMeshAgent.ResetPath(); // Останавливаем движение
            state = newState;
        }
    }

    private void HandleMovement()
    {
        Vector2 inputVector;


        inputVector = ((Vector2)currentDestination - rb.position).normalized;


        if (Mathf.Abs(inputVector.x) > Mathf.Abs(inputVector.y))
        {
            // Горизонтальное движение доминирует
            if (inputVector.x < 0)
            {
                isWalking = 3; // Движение влево
            }
            else if (inputVector.x > 0)
            {
                isWalking = 1; // Движение вправо
            }
        }
        else
        {
            // Вертикальное движение доминирует
            if (inputVector.y > 0)
            {
                isWalking = 4; // Движение вверх
            }
            else if (inputVector.y < 0)
            {
                isWalking = 2; // Движение вниз
            }
        }

        // Если вектор движения равен нулю, персонаж стоит
        if (inputVector == Vector2.zero)
        {
            isWalking = 0; // Стоит на месте
        }

        UpdateAnimations();
    }
    private void UpdateAnimations()
    {
        animator.SetInteger("IsWalking", isWalking);
    }
}

