using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerMovementInput;

[CreateAssetMenu(fileName = "Input Reader", menuName = "Input/Input Reader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    public event Action<bool> FierEvent;
    public event Action<Vector2> MoveEvent;
    public event Action<Vector2> MousePositionEvent;

    private PlayerMovementInput playerMovementInput;

    private void OnEnable()
    {
        if (playerMovementInput == null)
        {
            playerMovementInput = new PlayerMovementInput();
            playerMovementInput.Player.SetCallbacks(this);
        }
        playerMovementInput.Player.Enable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnMousePosition(InputAction.CallbackContext context)
    {
        MousePositionEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnFier(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            FierEvent?.Invoke(true);
        }
        else if (context.canceled)
        {
            FierEvent?.Invoke(false);
        }
    }
}
