using UnityEngine;

public class ItemUseManager : MonoBehaviour
{
    public static ItemUseManager Instance { get; private set; }
    public Door activeDoor;
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
            case 1: //  ÛÍÎ‡
                if (Ritual—ircle.Instance.playerInRange)
                {
                    Ritual—ircle.Instance.addDoll();
                    Inventory.Instance.ConsumeItem(itemId);
                }
                break;


            case 2: //  Î˛˜
                door(itemId);
                break;

            case 3: // ﬂ„Ó‰˚
                Healthbar.Instance.Heal(20); // ¬ÓÒÒÚ‡Ì‡‚ÎË‚‡ÂÏ Á‰ÓÓ‚¸Â
                Inventory.Instance.ConsumeItem(itemId);
                break;

            case 4: //  ÌË„‡
                Book.Instance.OnEnableBook(); // ŒÚÍ˚‚‡ÂÏ ÍÌË„Û
                break;

            case 5: // ÷‚ÂÚÓÍ
                flower();
                break;
            case 6: // ¬ÂÂ‚Í‡
                rope(itemId);
                break;
            case 7: // ’ÎÂ·

                break;
            case 8: // ¬Â‰Ó
                Cows.Instance.bucket();
                break;

            case 9: // ÃÓÎÓÍÓ

                break;

            case 10: // —‚Â˜‡
                if (Ritual—ircle.Instance != null)
                {
                    if (Ritual—ircle.Instance.playerInRange)
                    {
                        if (Inventory.Instance.GetItemCount(itemId) >= 5)
                        {
                            Ritual—ircle.Instance.addCandles();
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
            case 11: // ÃÂ¯ÓÍ
                if (Ritual—ircle.Instance.playerInRange)
                {
                    Ritual—ircle.Instance.addvictim();
                    Inventory.Instance.ConsumeItem(itemId);
                }
                break;
            case 12: // «‡ÔËÒÍ‡
                Note.Instance.OnEnableNote();
                break;

            default:
                Debug.Log("ÕÂËÁ‚ÂÒÚÌ˚È ÔÂ‰ÏÂÚ");
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



    private void door(int itemId)
    {
        if (activeDoor != null)
        {
            if (activeDoor.key == itemId)
            {
                activeDoor.UnlockDoor();
                Debug.Log("ƒ‚Â¸ ÓÚÍ˚Ú‡!");
                Inventory.Instance.ConsumeItem(itemId);
            }
            else
            {
                Debug.Log("›ÚÓÚ ÍÎ˛˜ ÌÂ ÔÓ‰ıÓ‰ËÚ ‰Îˇ ˝ÚÓÈ ‰‚ÂË.");
            }
        }
        else
        {
            Debug.Log("ÕÂÚ ‡ÍÚË‚ÌÓÈ ‰‚ÂË ‰Îˇ ÓÚÍ˚ÚËˇ.");
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