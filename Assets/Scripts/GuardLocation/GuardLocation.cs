using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardLocation : MonoBehaviour
{
    private BoxCollider2D Collider;
    [SerializeField] GameObject NoiseGame;
    [SerializeField] Transform Guards;
    [SerializeField] Animator animator;
    [SerializeField] TextAsset inkJSON;

    private void Awake()
    {
        Collider = GetComponent<BoxCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(StartGame());
        }
    }

    private IEnumerator StartGame()
    {
        GameInput.Instance.OnDisable();
        CameraController.changeFollowTargetEvent(Guards);
        animator.SetTrigger("start");
        yield return new WaitForSeconds(7f);
        CameraController.changeFollowTargetEvent(GameObject.Find("Player").transform);
        yield return new WaitForSeconds(0.5f);
        DialogueManager.Instance.StartDialog(inkJSON, "guard");
        while (DialogueManager.Instance.dialogPanelOpen)
        {
            yield return null;
        }
        GameInput.Instance.OnEnabled();
        Player.Instance.stealth = true;
        PlayerVisual.Instance.Stealth();
        Collider.enabled = false;
        NoiseGame.SetActive(true);
    }
}
