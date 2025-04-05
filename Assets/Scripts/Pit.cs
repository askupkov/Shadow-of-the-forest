using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pit : MonoBehaviour
{
    public static Pit Instance { get; private set; } 
    [SerializeField] Animator animator;
    [SerializeField] GameObject player;
    private Rigidbody2D rb;
    private int i = 0;
    private int j = 0;
    private bool playerInRange = false;
    [SerializeField] Transform position;
    [SerializeField] Transform destination;
    [SerializeField] Transform destination2;

    private float scrollSpeed = 0.1f; // Скорость движения фона
    private Material material;
    private Vector2 offset;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        rb = player.GetComponent<Rigidbody2D>();
        StartCoroutine(StartDown());
    }

    private IEnumerator StartDown()
    {
        animator.SetBool("DownRope", true);
        Player.Instance.StartToMove(destination);
        while (Player.Instance.isMovingToDestination)
        {
            yield return null;
        }
        animator.SetBool("DownRope", false);
    }
    private IEnumerator StartUp()
    {
        player.transform.position = position.position;
        animator.SetTrigger("UpRope");
        GameInput.Instance.OnDisable();
        Player.Instance.StartToMove(destination2);
        while (Player.Instance.isMovingToDestination)
        {
            yield return null;
        }
        GameInput.Instance.OnDisable();
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            material = renderer.material;
        }
        GaugeGame.Instance.StartGame();
    }


    void Update()
    {
        if (material != null)
        {
            offset.y += Time.deltaTime * scrollSpeed;
            material.mainTextureOffset = offset;
        }
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(StartUp());
        }
    }

    public void start_fall()
    {
        StartCoroutine(Fall());
    }
    private IEnumerator Fall()
    {
        if (i == 0)
        {
            animator.SetTrigger("AlmostFall");
            i++;
            j = 0;
            GaugeGame.Instance.speed = 500f;
            scrollSpeed = 0;
            yield return new WaitForSeconds(2f);
            GaugeGame.Instance.StartGame();
        }
        else
        {
            GaugeGame.Instance.gaugePanel.SetActive(false);
            animator.SetTrigger("Fall");
            scrollSpeed = 0;
            CameraController.Instance.FollowNull();
            rb.gravityScale = 80f;
            yield return new WaitForSeconds(1f);
            Destroy(player);
        }
    }

    public void start_succes()
    {
        StartCoroutine(Succes());
    }
    private IEnumerator Succes()
    {
        i = 0;
        j++;
        animator.SetTrigger("UpRope");
        scrollSpeed = 0.1f;
        if (j == 5)
        {
            CameraController.Instance.FollowNull();
            rb.gravityScale = -20f;
            GaugeGame.Instance.gaugePanel.SetActive(false);
            scrollSpeed = 0;
            SceneController.Instance.StartLoadScene(11);
        }
        else
        {
            yield return new WaitForSeconds(2f);
            GaugeGame.Instance.StartGame();
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
