using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private Animator animator;

    private const string IS_WALKING = "IsWalking";
    private const string IS_RUNNING = "IsRunning";
    private const string CANDLE = "Candle";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetInteger(IS_WALKING, Player.Instance.IsWalking());
        animator.SetInteger(IS_RUNNING, Player.Instance.IsRunning());
        animator.SetBool(CANDLE, Player.Instance.IsLighting());
    }
}

