using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    public static PlayerVisual Instance { get; private set; }
    private Animator animator;
    private AudioSource audioSource;

    private const string IS_WALKING = "IsWalking";
    private const string IS_RUNNING = "IsRunning";
    private const string CANDLE = "Candle";
    private const string DAMAGE = "Damage";

    private void Awake()
    {
        Instance = this;
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        AudioSetting.Instance.RegisterSfx(audioSource);
    }

    private void Update()
    {
        animator.SetInteger(IS_WALKING, Player.Instance.IsWalking());
        animator.SetInteger(IS_RUNNING, Player.Instance.IsRunning());
        animator.SetBool(CANDLE, Player.Instance.IsLighting());
    }

    public void TriggerDamage()
    {
        animator.SetTrigger(DAMAGE);
        audioSource.Play();
    }

    public void TriggerDie()
    {
        animator.SetBool("Die", true);
    }
    public void Stealth()
    {
        animator.SetTrigger("Stealth");
    }

    private IEnumerator Control()
    {
        GameInput.Instance.OnDisable();
        
        yield return new WaitForSeconds(0.5f);
        if(Healthbar.Instance.HP > 0)
        {
            GameInput.Instance.OnEnabled();
        }
    }
}

