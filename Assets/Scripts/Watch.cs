using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watch : MonoBehaviour
{
    public TextAsset inkJSON;
    public string startingPoint;
    private bool playerInRange = false;
    private BoxCollider2D Collider;
    private AudioSource audioSource;

    private void Start()
    {
        Collider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        AudioSetting.Instance.RegisterSfx(audioSource);
    }
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !Pause.Instance.pauseOpen)
        {
            StartCoroutine(watchCoroutine());
        }
    }

    private IEnumerator watchCoroutine()
    {
        DialogueManager.Instance.StartDialog(inkJSON, startingPoint);
        while (DialogueManager.Instance.dialogPanelOpen == true)
        {
            Collider.enabled = false;
            yield return null;
        }
        Collider.enabled = true;
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
