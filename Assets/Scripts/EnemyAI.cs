using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyAI : MonoBehaviour
{
    // Ссылка на игрока
    public Transform player;

    // Параметры моба
    public float detectionRange = 5f; // Дистанция обнаружения игрока
    public float attackRange = 1.5f;  // Дистанция атаки
    public float moveSpeed = 3f;      // Скорость передвижения

    // Временные переменные
    private bool isAttacking = false;
    private Rigidbody2D rb;

    void Start()
    {
        // Получаем компонент Rigidbody2D
        rb = GetComponent<Rigidbody2D>();

        // Находим игрока автоматически по тегу
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

        // Вычисляем расстояние до игрока
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Если игрок находится в зоне обнаружения, но вне зоны атаки
        if (distanceToPlayer <= detectionRange && distanceToPlayer > attackRange)
        {
            MoveTowardsPlayer();
            isAttacking = false;
        }
        // Если игрок находится в зоне атаки
        else if (distanceToPlayer <= attackRange)
        {
            AttackPlayer();
        }
        else
        {
            // Останавливаем движение, если игрок далеко
            StopMoving();
        }
    }

    void MoveTowardsPlayer()
    {
        // Вычисляем направление к игроку
        Vector2 direction = (player.position - transform.position).normalized;

        // Двигаемся в сторону игрока
        rb.velocity = direction * moveSpeed;
    }

    void AttackPlayer()
    {
        // Простая атака (например, выводим сообщение в консоль)
        if (!isAttacking)
        {
            isAttacking = true;

            Healthbar.Instance.TakeDamage(10);
        }
    }

    void StopMoving()
    {
        // Останавливаем движение
        rb.velocity = Vector2.zero;
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        // Визуализация зон обнаружения и атаки в редакторе
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}