using System.Collections;
using UnityEngine;

public class RoomGG : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] CapsuleCollider2D bedCollider;


    private void Start()
    {
        if (PlayerPrefs.GetInt(gameObject.name, 0) != 1)
        {
            StartCoroutine(startRoom());
        }
        else
        {
            bedCollider.enabled = true;
        }
    }
    private IEnumerator startRoom()
    {
        GameInput.Instance.OnDisable();
        animator.SetTrigger("Bed");
        yield return new WaitForSeconds(1.2f);
        Player.Instance.audioSource.Play();
        PlayerPrefs.SetInt(gameObject.name, 1);
        bedCollider.enabled = true;
        yield return new WaitForSeconds(0.5f);
        Management.Instance.OpenManagement();
        while (Management.Instance.ManagePanelOpen)
        {
            yield return null;
        }
        GameInput.Instance.OnEnabled();
    }
}
