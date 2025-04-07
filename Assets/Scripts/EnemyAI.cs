using System.Collections;
using System.Collections.Generic;
using Ink;
using UnityEngine;
using UnityEngine.AI;
using KnightAdventure.Utils;
using UnityEngine.EventSystems;
using System;
using UnityEngine.InputSystem.LowLevel;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private State startingState; // Начальное состояние
    [SerializeField] private bool isRoamingEnemy = false; // Включение/Отключение состояния брожения
    [SerializeField] private float roamingDistanceMax = 7f; // Максимальное расстояние брожения
    [SerializeField] private float roamingDistanceMin = 3f; // Минимальное расстояние брожения
    [SerializeField] private float roamingTimerMax = 2f; // Максимальное время брожения

    [SerializeField] private bool isChasingEnemy = false; // Включение/Отключение состояния приследования
    [SerializeField] private float chasingDistance = 4f;
    [SerializeField] private float chasingSpeedMultiplier = 2f;

    [SerializeField] private bool isAttackingEnemy = false; // Включение/Отключение состояния атаки
    [SerializeField] private float attackingDistance = 2f;
    [SerializeField] private float attackRate = 2f;

    [SerializeField] Animator animator;
    private float nextAttackTime = 0f;
    private int isWalking;

    private NavMeshAgent navMeshAgent;
    private State state;
    private float roamingTimer;
    private Vector3 roamPosition;
    private Vector3 startingPosition;

    private float roamingSpeed;
    private float chasingSpeed;

    public event EventHandler OnEnemyAttack;

    public bool IsRunning
    {
        get
        {
            if (navMeshAgent.velocity == Vector3.zero)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    private enum State
    {
        Idle,
        Roaming,
        Chasing,
        Attacking,
        Death
    }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        state = startingState;

        roamingSpeed = navMeshAgent.speed;
        chasingSpeed = navMeshAgent.speed;
        //chasingSpeed = navMeshAgent.speed * chasingSpeedMultiplier;
    }

    private void Update()
    {
        StateHandler();
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

            case State.Roaming: // Брожение
                roamingTimer -= Time.deltaTime;
                ChangeFacingDirection(startingPosition, roamPosition);
                if (roamingTimer < 0)
                {
                    Roaming();
                    roamingTimer = roamingTimerMax;
                }
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

            case State.Death: // Смерть
                StartCoroutine(Death());
                break;

        }
    }

    private void Idle()
    {
        animator.SetInteger("IsWalking", 0);
    }

    private void Roaming()
    {
        startingPosition = transform.position;
        roamPosition = GetRoamingPosition();
        navMeshAgent.SetDestination(roamPosition);
    }

    private void Chasing()
    {
        navMeshAgent.SetDestination(Player.Instance.transform.position);
    }

    private void Attacking()
    {
        if (Time.time > nextAttackTime)
        {
            animator.SetTrigger("Boom");
            OnEnemyAttack?.Invoke(this, EventArgs.Empty);
            nextAttackTime = Time.time + attackRate;
            
            Healthbar.Instance.TakeDamage(10);
        }

    }
    private IEnumerator Death()
    {
        yield return new WaitForSeconds(1.6f);
        //Destroy(gameObject);
    }

    private void CheckCurrentState() // Проверка состояния
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position);
        State newState = State.Idle;

        if (isRoamingEnemy)
        {
            newState = State.Roaming;
        }
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

        //if (newState != state)
        //{
        //    if (newState == State.Chasing)
        //    {
        //        navMeshAgent.ResetPath();
        //        navMeshAgent.speed = chasingSpeed;
        //    }
        //    else if (newState == State.Roaming)
        //    {
        //        roamingTimer = 0f;
        //        navMeshAgent.speed = roamingSpeed;
        //    }
        //    else if (newState == State.Roaming)
        //    {
        //        navMeshAgent.ResetPath();
        //    }

        //    state = newState;
        //}
        state = newState;
    }

    //private void CheckCurrentState() // Проверка состояния
    //{
    //    float distanceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position);
    //    State newState = State.Roaming;

    //    if (isChasingEnemy)
    //    {
    //        if (distanceToPlayer <= chasingDistance)
    //        {
    //            newState = State.Chasing;
    //        }
    //    }
    //    if (isAttackingEnemy)
    //    {
    //        if (distanceToPlayer <= attackingDistance)
    //        {
    //            newState = State.Attacking;
    //        }
    //    }

    //    if (newState != state)
    //    {
    //        if (newState == State.Chasing)
    //        {
    //            navMeshAgent.ResetPath();
    //            navMeshAgent.speed = chasingSpeed;
    //        }
    //        else if (newState == State.Roaming)
    //        {
    //            roamingTimer = 0f;
    //            navMeshAgent.speed = roamingSpeed;
    //        }
    //        else if (newState == State.Roaming)
    //        {
    //            navMeshAgent.ResetPath();
    //        }

    //        state = newState;
    //    }
    //}



    private Vector3 GetRoamingPosition()
    {
        return startingPosition + Utils.GetRandomDir() * UnityEngine.Random.Range(roamingDistanceMin, roamingDistanceMax);
    }

    private void ChangeFacingDirection(Vector3 sourcePosition, Vector3 targetPosition)
    {
        if (sourcePosition.y > targetPosition.y)
        {
            isWalking = 4;
        }
        else if (sourcePosition.y < targetPosition.y)
        {
            isWalking = 1;
        }
        else if(sourcePosition.x > targetPosition.x)
        {
            isWalking = 3;
        }
        else if (sourcePosition.x < targetPosition.x)
        {
            isWalking = 1;
        }
        UpdateAnimations();
    }
    private void UpdateAnimations()
    {
        animator.SetInteger("IsWalking", isWalking);
    }
}