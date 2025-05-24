using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    private PlayerInputActions playerInputActions;
    public bool panelOpen;

    private void Awake()
    {
        Instance = this;
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(Book.Instance.BookOpen)
            {
                Book.Instance.OnDisableBook();
            }
            else if (Inventory.Instance.InventoryOpen)
            {
                Inventory.Instance.CloseInventory();
            }
            else if (AudioSetting.Instance.settingOpen)
            {
                AudioSetting.Instance.closeAudioSetting();
            }
            else if(!DialogueManager.Instance.dialogPanelOpen && !panelOpen)
            {
                Pause.Instance.managePause();
            }
        }
    }

    public void OnEnabled()
    {
        playerInputActions.Enable(); // Включаем ввод
    }

    public void OnDisable()
    {
        playerInputActions.Disable(); // Отключаем ввод
    }

    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        return inputVector;
    }
}
