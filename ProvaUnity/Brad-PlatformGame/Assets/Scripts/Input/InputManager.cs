using System;
using UnityEngine.InputSystem;

public class InputManager
{
    private PlayerControls playercontrols;

    public float MoveDirection => playercontrols.Gameplay.Movement.ReadValue<float>();

    public event Action OnJump;
    public event Action OnAttack;
    
    public InputManager()
    {
        playercontrols = new PlayerControls();
        playercontrols.Gameplay.Enable();
        
        playercontrols.Gameplay.Jump.performed += OnJumpPerformed;
        playercontrols.Gameplay.Attack.performed += OnAttackPerformed;
    }

    private void OnAttackPerformed(InputAction.CallbackContext obj)
    {
        OnAttack?.Invoke();
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        OnJump?.Invoke();
    }

    public void DisableInput()
    {
        playercontrols.Gameplay.Disable();
    }
}
