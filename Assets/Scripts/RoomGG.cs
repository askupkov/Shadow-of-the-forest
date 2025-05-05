using System.Collections;
using UnityEngine;

public class RoomGG : MonoBehaviour
{
    [SerializeField] Animator animator; 
    [SerializeField] CapsuleCollider2D bedCollider;
    

    private void Start()
    {
        
        StartCoroutine(startRoom());
    }
    private IEnumerator startRoom()
    {
        GameInput.Instance.OnDisable();
        if (PlayerPrefs.GetInt(gameObject.name, 0) != 1)
        {
            animator.SetTrigger("Bed");
        }
        PlayerPrefs.SetInt(gameObject.name, 1);
        yield return new WaitForSeconds(1.4f);
        GameInput.Instance.OnEnabled();
        bedCollider.enabled = true;
    }
}
