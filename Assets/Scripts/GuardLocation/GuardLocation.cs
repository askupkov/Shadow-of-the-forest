using System.Collections;
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
        if (PlayerPrefs.GetInt(gameObject.name, 0) != 1)
        {
            foreach (var guard in GuardAI.guards)
            {
                if (guard.audioSource != null)
                {
                    guard.audioSource.mute = true;
                }
            }
            GameInput.Instance.OnDisable();
            CameraController.changeFollowTargetEvent(Guards);
            animator.SetTrigger("start");
            yield return new WaitForSeconds(7f);
            CameraController.changeFollowTargetEvent(GameObject.Find("Player").transform);
            yield return new WaitForSeconds(0.5f);
            DialogueManager.Instance.StartDialog(inkJSON, "guard1");
            while (DialogueManager.Instance.dialogPanelOpen)
            {
                yield return null;
            }
            GameInput.Instance.OnEnabled();
            foreach (var guard in GuardAI.guards)
            {
                if (guard.audioSource != null)
                {
                    guard.audioSource.mute = false;
                }
            }
        }
        PlayerPrefs.SetInt(gameObject.name, 1);
        Player.Instance.stealth = true;
        PlayerVisual.Instance.Stealth();
        Collider.enabled = false;
        NoiseGame.SetActive(true);
    }
}
