using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemUseManager : MonoBehaviour
{
    public static ItemUseManager Instance { get; private set; }
    public Door activeDoor; // Ссылка на текущую дверь
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
            case 1: // Кукла
                
                break;


            case 2: // Ключ
                door(itemId);
                break;

            case 3: // Ягоды
                Healthbar.Instance.Heal(20); // Восстанавливаем здоровье
                Inventory.Instance.ConsumeItem(itemId);
                break;

            case 4: // Книга
                Book.Instance.OnEnableBook(); // Открываем книгу
                break;

            case 5: // Цветок
                flower();
                break;
            case 6: // Веревка
                rope(itemId);
                break;
            case 7: // Хлеб
                
                break;
            case 8: // Ведро
                bucket();
                break;

            case 9: // Молоко

                break;

            case 10: // Свеча
                Player.Instance.Candle();
                break;

            default:
                Debug.Log("Неизвестный предмет");
                break;
        }
    }

    private void flower()
    {
        if (Swamp.Instance.playerInRange)
        {
            Inventory.Instance.ConsumeItem(5);
            Swamp.Instance.ritual();
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
                Debug.Log("Дверь открыта!");
                Inventory.Instance.ConsumeItem(itemId);
            }
            else
            {
                Debug.Log("Этот ключ не подходит для этой двери.");
            }
        }
        else
        {
            Debug.Log("Нет активной двери для открытия.");
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