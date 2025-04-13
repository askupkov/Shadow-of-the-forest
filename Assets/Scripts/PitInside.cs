using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UIElements;

public class PitInside : MonoBehaviour
{
    public static PitInside Instance { get; private set; }
    [SerializeField] Animator animator;
    [SerializeField] GameObject player;
    private Rigidbody2D rb;
    [SerializeField] TextAsset inkJSON;
    private int i = 0;
    private int j = 0;
    private bool playerInRange = false;
    [SerializeField] Transform position;
    [SerializeField] Transform destination;
    [SerializeField] Transform destination2;
    private BoxCollider2D Collider;
    [SerializeField] PolygonCollider2D polygonCollider;

    public bool visit = false;

    private float scrollSpeed = 0.1f; // Скорость движения фона
    private Material material;
    private Vector2 offset;

    private void Awake()
    {
        Instance = this;
        Collider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        rb = player.GetComponent<Rigidbody2D>();
        StartCoroutine(StartDown());
        visit = true;
    }

    private IEnumerator StartDown()
    {
        animator.SetBool("DownRope", true);
        Player.Instance.StartToMove(destination);
        while (Player.Instance.isMovingToDestination)
        {
            polygonCollider.enabled = false;
            yield return null;
        }
        polygonCollider.enabled = true;
        animator.SetBool("DownRope", false);
    }
    private IEnumerator StartUp()
    {
        if (Inventory.Instance.HasItem(5) == true)
        {
            polygonCollider.enabled = false;
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
        else
        {
            DialogueManager.Instance.StartDialog(inkJSON, "notleave");
            while (DialogueManager.Instance.dialogPanelOpen)
            {
                Collider.enabled = false;
                yield return null;
            }
            Collider.enabled = true;
        }
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
            CameraController.cameraShake?.Invoke(1f, 400f, 1f);
            animator.SetTrigger("AlmostFall");
            i++;
            j = 0;
            GaugeGame.Instance.speed = 1000f;
            scrollSpeed = 0;
            GaugeGame.Instance.failPanel.SetActive(true);
            yield return new WaitForSeconds(2f);
            GaugeGame.Instance.failPanel.SetActive(false);
            GaugeGame.Instance.StartGame();
        }
        else
        {
            CameraController.cameraShake?.Invoke(0f, 0f, 0f);
            GaugeGame.Instance.gaugePanel.SetActive(false);
            animator.SetTrigger("Fall");
            scrollSpeed = 0;
            CameraController.Instance.FollowNull();
            rb.gravityScale = 80f;
            yield return new WaitForSeconds(2f);
            Player.Instance.StartDie();
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
        CameraController.cameraShake?.Invoke(0f, 0f, 0f);
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
        else if (j == 1)
        {
            GaugeGame.Instance.speed = 800f;
            GaugeGame.Instance.victoryPanel.SetActive(true);
            yield return new WaitForSeconds(2f);
            GaugeGame.Instance.victoryPanel.SetActive(false);
            GaugeGame.Instance.StartGame();
        }
        else
        {
            GaugeGame.Instance.speed += 300f;
            GaugeGame.Instance.victoryPanel.SetActive(true);
            yield return new WaitForSeconds(2f);
            GaugeGame.Instance.victoryPanel.SetActive(false);
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
