using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victim : MonoBehaviour
{
    [SerializeField] Pick_Item pick_Item;
    [SerializeField] Watch watch;
    private void Update()
    {
        if(PlayerPrefs.GetInt("Victim", 0) == 1)
        {
            pick_Item.enabled = true;
            watch.enabled = false;
        }
    }
}
