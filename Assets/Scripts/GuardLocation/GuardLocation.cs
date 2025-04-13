using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardLocation : MonoBehaviour
{
    private BoxCollider2D Collider;

    private void Awake()
    {
        Collider = GetComponent<BoxCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player.Instance.stealth = true;
            PlayerVisual.Instance.Stealth();
            Collider.enabled = false;
        }
    }
}
