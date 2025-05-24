using System.Collections;
using UnityEngine;

public class RitualСircle : MonoBehaviour
{
    public static RitualСircle Instance {  get; private set; }
    public bool playerInRange;
    [SerializeField] Animator animator;
    BoxCollider2D Collider;
    [SerializeField] GameObject items;
    [SerializeField] GameObject candles;
    [SerializeField] GameObject doll;
    [SerializeField] GameObject victim;
    [SerializeField] GameObject leshiy;
    [SerializeField] TextAsset inkJSON;
    [SerializeField] Transform destination;
    [SerializeField] Transform destination2;
    [SerializeField] AudioClip[] sounds;

    [SerializeField] GameObject final_scene;
    private bool candlesRitual;
    private bool dollRitual;
    private bool victimRitual;
    private AudioSource audioSource;

    private bool startdialog = true;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        AudioSetting.Instance.RegisterSfx(audioSource);
        Collider = GetComponent<BoxCollider2D>();
        if (PlayerPrefs.GetInt(gameObject.name, 0) == 1)
        {
            startdialog = false;
        }
        if(PlayerPrefs.GetInt("candlesRitual", 0) == 1)
        {
            candles.SetActive(true);
            candlesRitual = true;
        }
        if(PlayerPrefs.GetInt("dollRitual", 0) == 1)
        {
            doll.SetActive(true);
            dollRitual = true;
        }
        if(PlayerPrefs.GetInt("victimRitual", 0) == 1)
        {
            victim.SetActive(true);
            victimRitual = true;
        }

        if(PlayerPrefs.GetInt("finalScene", 0) == 1)
        {
            StartCoroutine(final());
        }
    }

    private void Update()
    {
        if (playerInRange && startdialog)
        {
            DialogueManager.Instance.StartDialog(inkJSON, "leshiy");
            startdialog = false;
            PlayerPrefs.SetInt(gameObject.name, 1);
        }
    }

    public void addCandles()
    {
        audioSource.PlayOneShot(sounds[0]);
        candles.SetActive(true);
        PlayerPrefs.SetInt("candlesRitual", 1);
        candlesRitual = true;
    }

    public void addDoll()
    {
        audioSource.PlayOneShot(sounds[0]);
        doll.SetActive(true);
        dollRitual = true;
        PlayerPrefs.SetInt("dollRitual", 1);
    }

    public void addvictim()
    {
        audioSource.PlayOneShot(sounds[0]);
        victim.SetActive(true);
        PlayerPrefs.SetInt("victimRitual", 1);
        victimRitual = true;
    }

    public void startritual()
    {
        StartCoroutine(ritual());
    }

    private IEnumerator ritual()
    {
        if(candlesRitual && victimRitual && dollRitual)
        {
            Player.Instance.StartToMove(destination);
            while (Player.Instance.isMovingToDestination)
            {
                yield return null;
            }
            Player.Instance.StartToMove(destination2);
            while (Player.Instance.isMovingToDestination)
            {
                yield return null;
            }
            DialogueManager.Instance.StartDialog(inkJSON, "leshiy1");
            while (DialogueManager.Instance.dialogPanelOpen)
            {
                yield return null;
            }
            GameInput.Instance.OnDisable();
            Collider.enabled = false;
            audioSource.PlayOneShot(sounds[1]);
            animator.SetBool("Flash", true);
            yield return new WaitForSeconds(1.5f);
            leshiy.SetActive(true);
            Destroy(items);
            animator.SetBool("Flash", false);
            yield return new WaitForSeconds(0.5f);
            DialogueManager.Instance.StartDialog(inkJSON, "leshiy2");
            while (DialogueManager.Instance.dialogPanelOpen)
            {
                yield return null;
            }
            SceneController.Instance.StartLoadScene(18);
        }
    }
    
    private IEnumerator final()
    {
        Destroy(items);
        GameManager.Instance.OnDestroy();
        GameInput.Instance.OnDisable();
        final_scene.SetActive(true);
        CameraController.changeFollowTargetEvent(final_scene.transform);
        yield return new WaitForSeconds(5f);
        SceneController.Instance.StartLoadScene(0);
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
