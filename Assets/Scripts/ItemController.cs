using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public static ItemController Instance { get; private set; }
    public List<string> pickedItems = new List<string>();

    private void Awake()
    {
        Instance = this;
    }

    public void ClearPickedItems()
    {
        pickedItems.Clear();
    }

    public void addPickedItems(string name)
    {
        pickedItems.Add(name);
    }

    public void SaveScene()
    {
        foreach (string name in pickedItems)
        {
            PlayerPrefs.SetInt(name, 1);
        }
        PlayerPrefs.Save();
        ClearPickedItems();
    }

    public void SaveBush()
    {
        foreach (string name in pickedItems)
        {

            PlayerPrefs.SetInt(name, 0);
        }
        PlayerPrefs.Save();
        ClearPickedItems();
    }
}
