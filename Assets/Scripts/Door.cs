using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public static Door Instance { get; private set; }
    public int sceneToLoad;
    private Animator Animator;
    private bool playerInRange = false;
    public bool locked;
    public int key;
    private BoxCollider2D Collider;
    [SerializeField] TextAsset inkJSON;
    private string PlayerPrefsKey => $"{gameObject.name}";

    public Vector2 position;
    public VectorValue playerStorage;

    private void Awake()
    {
        Instance = this;
        Animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Collider = GetComponent<BoxCollider2D>();
        if (PlayerPrefs.HasKey(PlayerPrefsKey))
        {
            locked = PlayerPrefs.GetInt(PlayerPrefsKey) == 1;
        }
        else
        {
            PlayerPrefs.SetInt(PlayerPrefsKey, locked ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!locked) 
            {
                StartCoroutine(OpenDoorCoroutine());
            }
            else
            {
                DialogueManager.Instance.StartDialog(inkJSON, "door");
            }
        }
        if (DialogueManager.Instance.dialogPanelOpen == true)
        {
            Collider.enabled = false;
        }
        else
        {
            Collider.enabled = true;
        }
    }

    private IEnumerator OpenDoorCoroutine()
    {
        Animator.SetBool("Open", true);
        GameInput.Instance.OnDisable();
        playerStorage.initialValue = position;
        SceneFader.Instance.FadeToLevel();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneToLoad);
        SceneFader.Instance.FadeFromLevel();
    }

    public void UnlockDoor()
    {
        locked = false;
        PlayerPrefs.SetInt(PlayerPrefsKey, 0);
        PlayerPrefs.Save();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            ItemUseManager.Instance.activeDoor = this; // Устанавливаем ссылку на эту дверь
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            ItemUseManager.Instance.activeDoor = null; // Убираем ссылку на эту дверь
        }
    }
}

