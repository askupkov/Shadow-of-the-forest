using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InspectItem : MonoBehaviour
{
    public static InspectItem Instance { get; private set; }

    [SerializeField]  GameObject itemPanel; // ������ ��� ����������� ��������
    [SerializeField] Image Image; // ����������� ��������
    public GameObject Background;

    private void Awake()
    {
        Instance = this;
        itemPanel.SetActive(false); // �������� ������ �� ���������
    }

    public void ShowItem(Sprite itemImage)
    {
        Image.sprite = itemImage; // ������������� ������ ��������
        itemPanel.SetActive(true); // ���������� ������
    }

    public void HideItem()
    {
        itemPanel.SetActive(false); // �������� ������
        Background.SetActive(true);
    }
}
