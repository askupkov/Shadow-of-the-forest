using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InspectItem : MonoBehaviour
{
    public static InspectItem Instance { get; private set; }

    [SerializeField] private GameObject itemPanel; // ������ ��� ����������� ��������
    [SerializeField] private Image Image; // ����������� ��������

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
    }
}
