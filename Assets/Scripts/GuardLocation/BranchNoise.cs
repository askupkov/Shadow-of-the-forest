using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchNoise : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            NoiseManager.Instance.IncreaseNoise(1);
        }
    }
}
