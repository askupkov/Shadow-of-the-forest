using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStateManager : MonoBehaviour
{
    public string itemName;

    void Start()
    {
        itemName = Pick_Item.Instance.startingPoint;
        if (PlayerPrefs.GetInt(itemName, 0) == 1)
        {
            Destroy(gameObject);
        }
    }
    public void PickUpItem()
    {
        PlayerPrefs.SetInt(itemName, 1);
        PlayerPrefs.Save();
        Destroy(gameObject);
    }
}
