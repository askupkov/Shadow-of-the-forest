using System.Collections;
using UnityEngine;

public class Voices : MonoBehaviour
{
    private BoxCollider2D Collider;
    private SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;
    [SerializeField] TextAsset inkJSON;
    [SerializeField] GameObject Shadow;
    private AudioSource audioSource;

    private void Awake()
    {
        Collider = GetComponent<BoxCollider2D>();
        spriteRenderer = Shadow.GetComponent<SpriteRenderer>();
        if (PlayerPrefs.GetInt("Voices", 0) == 1)
        {
            Collider.enabled = false;
        }
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        AudioSetting.Instance.RegisterSfx(audioSource);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.Play();
            PlayerPrefs.SetInt("Voices", 1);
            StartCoroutine(startVoices());
            Collider.enabled = false;
        }
    }

    private IEnumerator startVoices()
    {
        DialogueManager.Instance.StartDialog(inkJSON, "voice1");
        while (DialogueManager.Instance.dialogPanelOpen)
        {
            yield return null;
        }
        GameInput.Instance.OnDisable();
        animator.SetTrigger("Voices1");
        yield return new WaitForSeconds(2f);
        spriteRenderer.sortingOrder = -3;
        DialogueManager.Instance.StartDialog(inkJSON, "voice2");
        while (DialogueManager.Instance.dialogPanelOpen)
        {
            yield return null;
        }
        GameInput.Instance.OnDisable();
        animator.SetTrigger("Voices2");
        yield return new WaitForSeconds(3f);
        DialogueManager.Instance.StartDialog(inkJSON, "voice3");
        while (DialogueManager.Instance.dialogPanelOpen)
        {
            yield return null;
        }
        audioSource.Stop();
        GameInput.Instance.OnDisable();
        animator.SetTrigger("Voices3");
        yield return new WaitForSeconds(1f);
        spriteRenderer.sortingOrder = 0;
        DialogueManager.Instance.StartDialog(inkJSON, "voice4");
        while (DialogueManager.Instance.dialogPanelOpen)
        {
            yield return null;
        }
    }
}
