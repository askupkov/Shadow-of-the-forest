using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Windmill : MonoBehaviour
{
    private Animator animator;
    [SerializeField] Transform FollowCamera;
    [SerializeField] PolygonCollider2D Collider;

    private void Start()
    {
        animator = FollowCamera.GetComponent<Animator>();
        if (PlayerPrefs.GetInt(gameObject.name, 0) == 1)
        {
            Collider.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(start());
        }
    }

    private IEnumerator start()
    {
        Collider.enabled = false;
        GameInput.Instance.OnDisable();
        CameraController.changeFollowTargetEvent(FollowCamera);
        animator.SetTrigger("start");
        yield return new WaitForSeconds(4f);
        CameraController.changeFollowTargetEvent(GameObject.Find("Player").transform);
        GameInput.Instance.OnEnabled();
        PlayerPrefs.SetInt(gameObject.name, 1);
    }
}
