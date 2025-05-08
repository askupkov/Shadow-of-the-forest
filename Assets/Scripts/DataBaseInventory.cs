using System.Collections.Generic;
using UnityEngine;

public class DataBaseInventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();
}

[System.Serializable]

public class Item
{
    public int id;
    public string name;
    public Sprite img;
    public Sprite img_insp;
}
