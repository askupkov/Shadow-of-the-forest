using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public static Spike Instance { get; private set; }

    private bool playerInRange;
    private float nextAttackTime = 0f;
    private float attackRate = 1f;

    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        if (playerInRange)
        {
            SpikesAttack();
        }
    }

    private void SpikesAttack()
    {
        if (Time.time > nextAttackTime)
        {
            PlayerVisual.Instance.TriggerDamage();
            Healthbar.Instance.TakeDamage(10);
            nextAttackTime = Time.time + attackRate;
        }
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
