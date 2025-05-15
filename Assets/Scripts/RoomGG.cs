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
        yield return new WaitForSeconds(1.4f);
        PlayerPrefs.SetInt(gameObject.name, 1);
        GameInput.Instance.OnEnabled();
        bedCollider.enabled = true;
    }
}
