using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance = null;

    public static InputManager Instance { get => _instance; private set => _instance = value; }

    private CharacterControls controls;

    public event Action OnJumpPerformed;
    public event Action OnInteractPerformed;
    public event Action OnInventoryPerformed;
    public event Action OnAttackPerformed;
    public event Action OnMovementPerformed;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;

        }
        controls = new CharacterControls();
    }

    private void OnEnable()
    {
        controls.Enable();

        controls.Pirata.Jump.performed += ctx => OnJumpPerformed?.Invoke();
        controls.Pirata.Interact.performed += ctx => OnInteractPerformed?.Invoke();
        controls.Pirata.Inventory.performed += ctx => OnInventoryPerformed?.Invoke();
        controls.Pirata.Attack.performed += ctx => OnAttackPerformed?.Invoke();
        controls.Pirata.Mover.performed += ctx => OnMovementPerformed?.Invoke();
        MoveKeyBindings();

    }

    private void OnDisable()
    {

        controls.Pirata.Jump.performed -= ctx => OnJumpPerformed?.Invoke();
        controls.Pirata.Interact.performed -= ctx => OnInteractPerformed?.Invoke();
        controls.Pirata.Inventory.performed -= ctx => OnInventoryPerformed?.Invoke();
        controls.Pirata.Attack.performed -= ctx => OnAttackPerformed?.Invoke();
        controls.Pirata.Mover.performed -= ctx => OnMovementPerformed?.Invoke();


        controls.Disable();
    }

    public bool LeftMouseClicked()
    {
        return controls.MoverBarco.ClickIzq.IsPressed();
    }

    public Vector2 MouseDelta()
    {

        return controls.MoverBarco.MouseDelta.ReadValue<Vector2>();
    }

    public Vector2 MoveDirection()
    {

        return controls.Pirata.Mover.ReadValue<Vector2>();
    }

    public bool isJumpPressed()
    {
        return controls.Pirata.Jump.IsPressed();
    }

    public bool Interact()
    {
        return controls.Pirata.Interact.IsPressed();
    }
    public Vector2 MouseDeltaPirata()
    {
        return controls.Pirata.MoverCamara.ReadValue<Vector2>();
    }

    public bool AttackIsPressed()
    {
        return controls.Pirata.Attack.IsPressed();
    }

    public bool InventoryPressed()
    {
        return controls.Pirata.Inventory.IsPressed();
    }

    [HideInInspector]public string upKey;
    [HideInInspector]public string downKey;
    [HideInInspector]public string leftKey;
    [HideInInspector]public string rightKey;


    public void MoveKeyBindings()
    {
        int upIdx = controls.Pirata.Mover.bindings.IndexOf(b => b.name == "up");
        int downIdx = controls.Pirata.Mover.bindings.IndexOf(b => b.name == "down");
        int leftIdx = controls.Pirata.Mover.bindings.IndexOf(b => b.name == "left");
        int rightIdx = controls.Pirata.Mover.bindings.IndexOf(b => b.name == "right");

        
        upKey = controls.Pirata.Mover.GetBindingDisplayString(upIdx);
        downKey = controls.Pirata.Mover.GetBindingDisplayString(downIdx);
        leftKey = controls.Pirata.Mover.GetBindingDisplayString(leftIdx);
        rightKey = controls.Pirata.Mover.GetBindingDisplayString(rightIdx);

        Debug.Log($"Controls: {upKey}/{leftKey}/{downKey}/{rightKey}");
    }
}
