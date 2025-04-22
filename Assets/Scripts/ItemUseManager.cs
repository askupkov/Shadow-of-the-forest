using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemUseManager : MonoBehaviour
{
    public static ItemUseManager Instance { get; private set; }
    public Door activeDoor; // ������ �� ������� �����
    private bool playerInRange;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void UseItem(int itemId, Inventory inventory)
    {
        switch (itemId)
        {
            case 1: // �����
                if (Ritual�ircle.Instance.playerInRange)
                {
                    Ritual�ircle.Instance.addDoll();
                    Inventory.Instance.ConsumeItem(itemId);
                }
                break;


            case 2: // ����
                door(itemId);
                break;

            case 3: // �����
                Healthbar.Instance.Heal(20); // ��������������� ��������
                Inventory.Instance.ConsumeItem(itemId);
                break;

            case 4: // �����
                Book.Instance.OnEnableBook(); // ��������� �����
                break;

            case 5: // ������
                flower();
                break;
            case 6: // �������
                rope(itemId);
                break;
            case 7: // ����

                break;
            case 8: // �����
                bucket();
                break;

            case 9: // ������

                break;

            case 10: // �����
                if (Ritual�ircle.Instance != null)
                {
                    if (Ritual�ircle.Instance.playerInRange)
                    {
                        if (Inventory.Instance.GetItemCount(itemId) >= 5)
                        {
                            Ritual�ircle.Instance.addCandles();
                            for (int i = 0; i < 5; i++)
                            {
                                Inventory.Instance.ConsumeItem(itemId);
                            }

                        }
                    }
                }
                else
                {
                    Player.Instance.Candle();
                }
                break;
            case 11: // �����
                if (Ritual�ircle.Instance.playerInRange)
                {
                    Ritual�ircle.Instance.addvictim();
                    Inventory.Instance.ConsumeItem(itemId);
                }
                break;
            case 12: // �������
                Note.Instance.OnEnableNote();
                break;

            default:
                Debug.Log("����������� �������");
                break;
        }
    }

    private void flower()
    {
        if (Swamp.Instance.playerInRange)
        {
            Inventory.Instance.ConsumeItem(5);
            Swamp.Instance.startFlower();
        }
    }

    private void bucket()
    {
        if (Cows.Instance.playerInRange)
        {
            Inventory.Instance.ConsumeItem(8);
            Inventory.Instance.AddItem(9);
        }
    }

    private void door(int itemId)
    {
        if (activeDoor != null)
        {
            if (activeDoor.key == itemId)
            {
                activeDoor.UnlockDoor();
                Debug.Log("����� �������!");
                Inventory.Instance.ConsumeItem(itemId);
            }
            else
            {
                Debug.Log("���� ���� �� �������� ��� ���� �����.");
            }
        }
        else
        {
            Debug.Log("��� �������� ����� ��� ��������.");
        }
    }

    private void rope(int itemId)
    {
        if (Pit.Instance.playerInCollider2Range == true)
        {
            Pit.Instance.Withrope();
            Inventory.Instance.ConsumeItem(itemId);
        }
    }
}