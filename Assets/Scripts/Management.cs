using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Management : MonoBehaviour
{
    public static Management Instance { get; private set; }
    public GameObject ManagementPanel;
    public bool ManagePanelOpen = false;

    private void Awake()
    {
        Instance = this;
        ManagementPanel.SetActive(false);
    }

    public void OpenManagement()
    {
        ManagementPanel.SetActive(!ManagePanelOpen);
        ManagePanelOpen = !ManagePanelOpen;
    }
}
