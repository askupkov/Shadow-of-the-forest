using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class Swamp : MonoBehaviour
{
    public static Swamp Instance { get; private set; }
    public bool playerInRange;
    [SerializeField] PolygonCollider2D lilies;
    [SerializeField] GameObject swampItem;
    [SerializeField] GameObject swampGlass;
    [SerializeField] Animator vodyanoyAnim;
    [SerializeField] Animator liliesAnim;
    [SerializeField] Animator flowerAnim;

    [SerializeField] Animator coinAnim;
    [SerializeField] Animator oberegAnim;
    [SerializeField] Animator stoneAnim;

    [SerializeField] Animator loveAnim;
    [SerializeField] Animator strahAnim;
    [SerializeField] Animator vlastAnim;
    [SerializeField] TextAsset inkJSON;

    [SerializeField] Transform destination1;
    [SerializeField] Transform destination2;
    private bool second_ritual;
    private bool passed;
    private bool flower;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt(gameObject.name, 0) == 1)
        {
            liliesAnim.SetTrigger("lilies");
            lilies.enabled = false;
        }
    }

    private void Update()
    {
        if(playerInRange && PlayerPrefs.GetInt("passed_swamp", 0) != 1)
        {
            DialogueManager.Instance.StartDialog(inkJSON, "swamp1");
            PlayerPrefs.SetInt("passed_swamp", 1);
        }
    }

    public void start_ritual()
    {
        StartCoroutine(ritual());
        PlayerPrefs.SetInt(gameObject.name, 1);
    }

    public void startFlower()
    {
        flowerAnim.SetTrigger("flower");
        flower = true;
    }

    private IEnumerator ritual()
    {
        if (flower)
        {
            GameInput.Instance.OnDisable();
            flowerAnim.SetTrigger("ritual");
            StartCoroutine(player());
            yield return new WaitForSeconds(2f);
            if (second_ritual)
            {
                DialogueManager.Instance.StartDialog(inkJSON, "swampsecond");
                while (DialogueManager.Instance.dialogPanelOpen)
                {
                    yield return null;
                }
                StartCoroutine(glass());
            }
            else
            {
                DialogueManager.Instance.StartDialog(inkJSON, "swamp3");
                while (DialogueManager.Instance.dialogPanelOpen)
                {
                    yield return null;
                }
                swampItem.SetActive(true);
                coinAnim.SetTrigger("TriggerItem");
                DialogueManager.Instance.StartDialog(inkJSON, "coin");
                while (DialogueManager.Instance.dialogPanelOpen)
                {
                    yield return null;
                }
                oberegAnim.SetTrigger("TriggerItem");
                DialogueManager.Instance.StartDialog(inkJSON, "obereg");
                while (DialogueManager.Instance.dialogPanelOpen)
                {
                    yield return null;
                }
                stoneAnim.SetTrigger("TriggerItem");
                DialogueManager.Instance.StartDialog(inkJSON, "stone");
                while (DialogueManager.Instance.dialogPanelOpen)
                {
                    yield return null;
                }
            }
        }
        flower = false;
    }
    private IEnumerator player()
    {
        Player.Instance.StartToMove(destination1);
        while (Player.Instance.isMovingToDestination)
        {
            yield return null;
        }
        Player.Instance.StartToMove(destination2);
        while (Player.Instance.isMovingToDestination)
        {
            yield return null;
        }
    }

    private IEnumerator glass()
    {
        swampGlass.SetActive(true);
        DialogueManager.Instance.StartDialog(inkJSON, "glass");
        while (DialogueManager.Instance.dialogPanelOpen)
        {
            yield return null;
        }
        loveAnim.SetTrigger("TriggerItem");
        DialogueManager.Instance.StartDialog(inkJSON, "love");
        while (DialogueManager.Instance.dialogPanelOpen)
        {
            yield return null;
        }
        strahAnim.SetTrigger("TriggerItem");
        DialogueManager.Instance.StartDialog(inkJSON, "strah");
        while (DialogueManager.Instance.dialogPanelOpen)
        {
            yield return null;
        }
        vlastAnim.SetTrigger("TriggerItem");
        DialogueManager.Instance.StartDialog(inkJSON, "vlast");
        while (DialogueManager.Instance.dialogPanelOpen)
        {
            yield return null;
        }
    }


    public void startItem(string item)
    {
        switch (item)
        {
            case "coin":
                StartCoroutine(coin_answer());
                break;

            case "obereg":
                StartCoroutine(obereg_answer());
                break;

            case "stone":
                StartCoroutine(stone_answer());
                break;

            case "love":
                StartCoroutine(love_answer());
                break;

            case "strah":
                StartCoroutine(strah_answer());
                break;

            case "vlast":
                StartCoroutine(vlast_answer());
                break;
        }
    }

    private IEnumerator coin_answer()
    {
        swampItem.SetActive(false);
        DialogueManager.Instance.StartDialog(inkJSON, "coin_answer");
        while (DialogueManager.Instance.dialogPanelOpen)
        {
            yield return null;
        }
        second_ritual = true;
    }

    private IEnumerator obereg_answer()
    {
        swampItem.SetActive(false);
        DialogueManager.Instance.StartDialog(inkJSON, "obereg_answer");
        while (DialogueManager.Instance.dialogPanelOpen)
        {
            yield return null;
        }
        StartCoroutine(player());
        yield return new WaitForSeconds(1f);
        StartCoroutine(glass());
    }

    private IEnumerator stone_answer()
    {
        swampItem.SetActive(false);
        DialogueManager.Instance.StartDialog(inkJSON, "stone_answer");
        while (DialogueManager.Instance.dialogPanelOpen)
        {
            yield return null;
        }
        StartCoroutine(final());
    }

    private IEnumerator final()
    {
        swampGlass.SetActive(false);
        vodyanoyAnim.SetTrigger("poyavlenie");
        yield return new WaitForSeconds(2f);
        DialogueManager.Instance.StartDialog(inkJSON, "final");
        while (DialogueManager.Instance.dialogPanelOpen)
        {
            yield return null;
        }
        vodyanoyAnim.SetTrigger("ischeznovenie");
        yield return new WaitForSeconds(1f);
        liliesAnim.SetTrigger("lilies");
        lilies.enabled = false;
    }
    private IEnumerator love_answer()
    {
        swampItem.SetActive(false);
        DialogueManager.Instance.StartDialog(inkJSON, "love_answer");
        while (DialogueManager.Instance.dialogPanelOpen)
        {
            yield return null;
        }
        StartCoroutine(final());
    }
    private IEnumerator strah_answer()
    {
        swampItem.SetActive(false);
        DialogueManager.Instance.StartDialog(inkJSON, "strah_answer");
        while (DialogueManager.Instance.dialogPanelOpen)
        {
            yield return null;
        }
    }
    private IEnumerator vlast_answer()
    {
        swampItem.SetActive(false);
        DialogueManager.Instance.StartDialog(inkJSON, "vlast_answer");
        while (DialogueManager.Instance.dialogPanelOpen)
        {
            yield return null;
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
