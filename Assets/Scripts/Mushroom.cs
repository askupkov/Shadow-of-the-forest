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
    [SerializeField] private State startingState; // ��������� ���������

    [SerializeField] private bool isChasingEnemy = false; // ���������/���������� ��������� �������������
    [SerializeField] private float chasingDistance = 4f;

    [SerializeField] private bool isAttackingEnemy = false; // ���������/���������� ��������� �����
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
            case State.Idle: // �����
                Idle();
                CheckCurrentState();
                break;

            case State.Chasing: // �������������
                Chasing();
                CheckCurrentState();
                break;

            case State.Attacking: // �����
                Attacking();
                state = State.Death;
                break;

            case State.Death: // ������
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

    private void CheckCurrentState() // �������� ���������
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
            navMeshAgent.ResetPath(); // ������������� ��������
            state = newState;
        }
    }
}