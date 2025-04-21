using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] GameObject final_scene;
    private bool candlesRitual;
    private bool dollRitual;
    private bool victimRitual;

    private bool startdialog = true;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
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
        candles.SetActive(true);
        PlayerPrefs.SetInt("candlesRitual", 1);
        candlesRitual = true;
    }

    public void addDoll()
    {
        doll.SetActive(true);
        dollRitual = true;
        PlayerPrefs.SetInt("dollRitual", 1);
    }

    public void addvictim()
    {
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
            DialogueManager.Instance.StartDialog(inkJSON, "leshiy1");
            while (DialogueManager.Instance.dialogPanelOpen)
            {
                yield return null;
            }
            Collider.enabled = false;
            animator.SetBool("Flash", true);
            yield return new WaitForSeconds(1.5f);
            leshiy.SetActive(true);
            Destroy(items);
            animator.SetBool("Flash", false);
            yield return new WaitForSeconds(0.5f);
            DialogueManager.Instance.StartDialog(inkJSON, "leshiy2");
        }
    }
    
    private IEnumerator final()
    {
        GameInput.Instance.OnDisable();
        final_scene.SetActive(true);
        CameraController.changeFollowTargetEvent(final_scene.transform);
        yield return new WaitForSeconds(5f);
        SceneFader.Instance.FadeToLevel();
        yield return new WaitForSeconds(3);
        GameManager.Instance.ReturnToMainMenu();
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
