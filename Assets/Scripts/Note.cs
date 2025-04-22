using System.Collections;
using UnityEngine;

public class Note : MonoBehaviour
{
    public static Note Instance { get; private set; }
    public GameObject note;
    public bool noteOpen;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        note.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && noteOpen)
        {
            OnDisableNote();
        }
    }

    public void OnEnableNote()
    {
        noteOpen = true;
        note.SetActive(true);
        StartCoroutine(InputDisabled());
    }

    private IEnumerator InputDisabled()
    {
        yield return new WaitForSeconds(0.4f);
        GameInput.Instance.OnDisable();
    }

    public void OnDisableNote()
    {
        GameInput.Instance.OnEnabled();
        noteOpen = false;
        note.SetActive(false);
    }

    public void read()
    {
        GameInput.Instance.OnEnabled();
        if (Swamp.Instance != null)
        {
            if (Swamp.Instance.playerInRange)
            {
                Swamp.Instance.start_ritual();
            }
        }
        noteOpen = false;
        note.SetActive(false);
    }
}
