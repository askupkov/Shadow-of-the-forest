using System.Collections;
using System.Collections.Generic;
using Ink;
using UnityEngine;
using UnityEngine.AI;
using KnightAdventure.Utils;
using UnityEngine.EventSystems;
using System;
using UnityEngine.InputSystem.LowLevel;

public class Mushroom : MonoBehaviour
{
    public static Mushroom Instance { get; private set; }
    [SerializeField] private State startingState; // Начальное состояние

    [SerializeField] private bool isChasingEnemy = false; // Включение/Отключение состояния приследования
    [SerializeField] private float chasingDistance = 4f;

    [SerializeField] private bool isAttackingEnemy = false; // Включение/Отключение состояния атаки
    [SerializeField] private float attackingDistance = 2f;
    [SerializeField] private float attackRate = 2f;

    [SerializeField] Animator animator;
    private float nextAttackTime = 0f;
    private int isWalking;

    private NavMeshAgent navMeshAgent;
    private State state;
    [SerializeField] bool death;
    Rigidbody2D rb;
    [SerializeField] bool damage;
    private bool isDamageTriggered = false;

    private enum State
    {
        Idle,
        Chasing,
        Attacking,
        Death
    }

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        state = startingState;

    }

    private void Update()
    {
        StateHandler();
        if (death)
        {
            Destroy(gameObject);
        }
        if (damage && !isDamageTriggered)
        {
            PlayerVisual.Instance.TriggerDamage();
            isDamageTriggered = true;
        }
        if (!damage)
        {
            isDamageTriggered = false;
        }
    }


    private void StateHandler()
    {
        switch (state)
        {
            default:
            case State.Idle: // Покой
                Idle();
                CheckCurrentState();
                break;

            case State.Chasing: // Приследование
                Chasing();
                CheckCurrentState();
                break;

            case State.Attacking: // Атака
                Attacking();
                state = State.Death;
                break;
            case State.Death:

                break;
        }
    }

    private void Idle()
    {
        animator.SetInteger("IsWalking", 0);
    }

    private void Chasing()
    {
        HandleMovement();
        navMeshAgent.SetDestination(Player.Instance.transform.position);
    }

    private void Attacking()
    {
        
        if (Time.time > nextAttackTime)
        {
           
            animator.SetInteger("IsWalking", 5);

            nextAttackTime = Time.time + attackRate;
            Healthbar.Instance.TakeDamage(10);
        }
    }

    private void CheckCurrentState() // Проверка состояния
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position);
        State newState = State.Idle;

        if (isChasingEnemy)
        {
            if (distanceToPlayer <= chasingDistance)
            {
                newState = State.Chasing;
            }
        }
        if (isAttackingEnemy)
        {
            if (distanceToPlayer <= attackingDistance)
            {
                newState = State.Attacking;
            }
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

        inputVector = ((Vector2)Player.Instance.transform.position - rb.position).normalized;

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
        UpdateAnimations();
    }

    private void UpdateAnimations()
    {
        animator.SetInteger("IsWalking", isWalking);
    }
}