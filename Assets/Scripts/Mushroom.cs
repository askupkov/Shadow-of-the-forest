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


    private enum State
    {
        Idle,
        Chasing,
        Attacking,
        Death
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        state = startingState;

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
        animator.SetBool("Run", false);
    }

    private void Chasing()
    {
        animator.SetBool("Run", true);
        navMeshAgent.SetDestination(Player.Instance.transform.position);
    }

    private void Attacking()
    {
        if (Time.time > nextAttackTime)
        {
            animator.SetTrigger("Boom");
            nextAttackTime = Time.time + attackRate;
            Healthbar.Instance.TakeDamage(10);
        }
    }
    private IEnumerator Death()
    {
        yield return new WaitForSeconds(1.6f);
        Destroy(gameObject);
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
}