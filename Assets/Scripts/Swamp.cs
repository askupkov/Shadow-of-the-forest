using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swamp : MonoBehaviour
{
    public static Swamp Instance { get; private set; }
    public bool playerInRange;
    Animator floweranim;

    private void Awake()
    {
        Instance = this;
    }

    public void ritual()
    {
        floweranim.SetTrigger("ritual");
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
