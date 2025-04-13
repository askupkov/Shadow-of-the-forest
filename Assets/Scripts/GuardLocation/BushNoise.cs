using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushNoise : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            BushManager.Instance.PlayerEnteredBush(this);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            BushManager.Instance.PlayerExitedBush(this);
        }
    }
}
