using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    private bool playerInRange = false;
    private BoxCollider2D Collider;
    [SerializeField] Transform destination1;
    [SerializeField] Transform destination2;
    [SerializeField] Transform destination3;
    [SerializeField] GameObject mouse;
    [SerializeField] TextAsset inkJSON;
    [SerializeField] Animator animator;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        AudioSetting.Instance.RegisterSfx(audioSource);
        Collider = GetComponent<BoxCollider2D>();
        if(PlayerPrefs.GetInt("HouseGrigoriy", 0) == 1)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (playerInRange && Player.Instance.lighting)
        {
            Collider.enabled = false;
            StartCoroutine(mouse_run());
            playerInRange = false;
        }
    }
    IEnumerator mouse_run()
    {
        GameInput.Instance.OnDisable();
        yield return new WaitForSeconds(1f);
        audioSource.Play();
        NPC.Instance.StartToMove(destination1);
        while (NPC.Instance.isMovingToDestination)
        {
            yield return null;
        }
        NPC.Instance.StartToMove(destination2);
        while (NPC.Instance.isMovingToDestination)
        {
            yield return null;
        }
        NPC.Instance.StartToMove(destination3);
        while (NPC.Instance.isMovingToDestination)
        {
            yield return null;
        }
        Destroy(mouse);
        DialogueManager.Instance.StartDialog(inkJSON, "house_grigoriy");
        while (DialogueManager.Instance.dialogPanelOpen)
        {
            yield return null;
        }
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
