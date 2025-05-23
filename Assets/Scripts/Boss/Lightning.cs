using System.Collections;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    public static Lightning Instance { get; private set; }
    [SerializeField] private GameObject LightningPrefab;
    private float shadowSpeed = 3f;
    private AudioSource audioSource;

    private Transform player;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        player = Player.Instance.transform;
        audioSource = GetComponent<AudioSource>();
        AudioSetting.Instance.RegisterSfx(audioSource);
    }

    public void TriggerLightningAttack()
    {
        StartCoroutine(LightningAttackRoutine());
    }

    private IEnumerator LightningAttackRoutine()
    {
        GameObject currentLightning = Instantiate(LightningPrefab, transform.position, Quaternion.identity);
        Rigidbody2D LightningRb = currentLightning.GetComponent<Rigidbody2D>();
        Animator animator = currentLightning.GetComponent <Animator>();
        yield return new WaitForSeconds(0.7f);

        while (Vector2.Distance(currentLightning.transform.position, player.position) > 0.1f)
        {
            Vector2 direction = (player.position - currentLightning.transform.position).normalized;
            LightningRb.velocity = direction * shadowSpeed;
            yield return null;
        }

        LightningRb.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.3f);
        animator.SetTrigger("Hit");
        audioSource.Play();
        if (Vector2.Distance(player.position, currentLightning.transform.position) < 1f)
        {
            PlayerVisual.Instance.TriggerDamage();
            Healthbar.Instance.TakeDamage(20);
        }
        yield return new WaitForSeconds(2f);
        Destroy(currentLightning);
    }
}
