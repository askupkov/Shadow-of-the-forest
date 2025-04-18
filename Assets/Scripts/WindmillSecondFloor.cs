using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindmillSecondFloor : MonoBehaviour
{
    [SerializeField] TextAsset inkJSON;
    private bool secondDialog;
    private void Start()
    {
        if (PlayerPrefs.GetInt(gameObject.name, 0) == 1)
        {
            secondDialog = true;
        }
        if (!secondDialog)
        {
            StartCoroutine(firstDialogue());
        }
        else
        {
            StartCoroutine(secondDialogue());
        }
    }

    private IEnumerator firstDialogue()
    {
        GameInput.Instance.OnDisable();
        DialogueManager.Instance.StartDialog(inkJSON, "domovoy");
        while (DialogueManager.Instance.dialogPanelOpen)
        {
            yield return null;
        }
        PlayerPrefs.SetInt(gameObject.name, 1);
        SceneController.Instance.StartLoadScene(14);
    }
    private IEnumerator secondDialogue()
    {
        GameInput.Instance.OnDisable();
        DialogueManager.Instance.StartDialog(inkJSON, "domovoy1");
        while (DialogueManager.Instance.dialogPanelOpen)
        {
            yield return null;
        }
        SceneController.Instance.StartLoadScene(14);
    }
}
