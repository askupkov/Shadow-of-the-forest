using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInput : MonoBehaviour
{
    public bool shift = false;
    public static GameInput Instance { get; private set; }

    private PlayerInputActions playerInputActions;
    private void Awake()
    {
        Instance = this;
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
    }

    public void OnEnabled()
    {
        playerInputActions.Enable(); // �������� ����
    }

    public void OnDisable()
    {
        playerInputActions.Disable(); // ��������� ����
    }

    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        return inputVector;
    }



}
