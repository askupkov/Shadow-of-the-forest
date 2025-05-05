using UnityEngine;

public class BushManager : MonoBehaviour
{
    public static BushManager Instance { get; private set; }

    private BushNoise currentBush;
    public bool PlayerHidden => currentBush != null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void PlayerEnteredBush(BushNoise bush)
    {
        currentBush = bush;
    }

    public void PlayerExitedBush(BushNoise bush)
    {
        if (currentBush == bush)
        {
            currentBush = null;
        }
    }
}
