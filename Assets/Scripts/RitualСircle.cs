using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RitualСircle : MonoBehaviour
{
    private bool playerInRange;
    [SerializeField] Animator animator;
    BoxCollider2D Collider;
    [SerializeField] GameObject items;

    private void Start()
    {
        Collider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (playerInRange)
        {
            StartCoroutine(enumerator());
        }
    }

    private IEnumerator enumerator()
    {
        Collider.enabled = false;
        animator.SetBool("Flash",true);
        yield return new WaitForSeconds(1.5f);
        Destroy(items);
        animator.SetBool("Flash", false);
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
