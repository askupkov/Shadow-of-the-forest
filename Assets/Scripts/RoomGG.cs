using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGG : MonoBehaviour
{
    [SerializeField] Animator animator;

    private void Start()
    {
        animator.SetTrigger("Bed");
    }
}
