using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyAI : MonoBehaviour
{
    // ������ �� ������
    public Transform player;

    // ��������� ����
    public float detectionRange = 5f; // ��������� ����������� ������
    public float attackRange = 1.5f;  // ��������� �����
    public float moveSpeed = 3f;      // �������� ������������

    // ��������� ����������
    private bool isAttacking = false;
    private Rigidbody2D rb;

    void Start()
    {
        // �������� ��������� Rigidbody2D
        rb = GetComponent<Rigidbody2D>();

        // ������� ������ ������������� �� ����
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
    }

    void Update()
    {
        if (player == null) return;

        // ��������� ���������� �� ������
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // ���� ����� ��������� � ���� �����������, �� ��� ���� �����
        if (distanceToPlayer <= detectionRange && distanceToPlayer > attackRange)
        {
            MoveTowardsPlayer();
            isAttacking = false;
        }
        // ���� ����� ��������� � ���� �����
        else if (distanceToPlayer <= attackRange)
        {
            AttackPlayer();
        }
        else
        {
            // ������������� ��������, ���� ����� ������
            StopMoving();
        }
    }

    void MoveTowardsPlayer()
    {
        // ��������� ����������� � ������
        Vector2 direction = (player.position - transform.position).normalized;

        // ��������� � ������� ������
        rb.velocity = direction * moveSpeed;
    }

    void AttackPlayer()
    {
        // ������� ����� (��������, ������� ��������� � �������)
        if (!isAttacking)
        {
            isAttacking = true;

            Healthbar.Instance.TakeDamage(10);
        }
    }

    void StopMoving()
    {
        // ������������� ��������
        rb.velocity = Vector2.zero;
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        // ������������ ��� ����������� � ����� � ���������
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}