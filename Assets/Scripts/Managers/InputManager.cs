using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class InputManager : Singleton<InputManager>
{
    [SerializeField]
    private InputActionReference MovementAction = null;
    [SerializeField]
    private InputActionReference JumpAction = null;
    [SerializeField]
    private InputActionReference MouseLeftClickAction = null;

    public Vector3 movementInput { get; private set; }
    public bool mouseLeftClick { get; private set; }

    private void Update()
    {
        Vector2 MoveInput = MovementAction.action.ReadValue<Vector2>();
        movementInput = new Vector3(MoveInput.x,0,MoveInput.y);
    }

    public void RegisterOnJumpAction(Action<InputAction.CallbackContext> OnJumpAction, bool register)
    {
        if(register)
            JumpAction.action.performed += OnJumpAction;
        else
            JumpAction.action.performed -= OnJumpAction;
    }

    public void RegisterOnMouseLeftClicked(Action<InputAction.CallbackContext> OnMouseLeftAction, bool register)
    {
        if (register)
            MouseLeftClickAction.action.performed += OnMouseLeftAction;
        else
            MouseLeftClickAction.action.performed -= OnMouseLeftAction;
    }

    public override void Reload()
    {
    }

}
