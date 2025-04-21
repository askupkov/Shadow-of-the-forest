using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

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
        Inventory.Instance.ConsumeItem(7);
        Inventory.Instance.ConsumeItem(9);
        while (DialogueManager.Instance.dialogPanelOpen)
        {
            yield return null;
        }
        DialogueManager.Instance.StartDialog(inkJSON, "domovoy2");
        while (DialogueManager.Instance.dialogPanelOpen)
        {
            yield return null;
        }
        for (int i = 0; i < 3; i++)
        {
            Inventory.Instance.AddItem(10);
        }
        SceneController.Instance.StartLoadScene(14);
    }
}
