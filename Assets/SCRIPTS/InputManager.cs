using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance = null;

    public static InputManager Instance { get => _instance; private set => _instance = value; }

    private CharacterControls controls;

    public event Action OnJumpPerformed;
    public event Action OnInteractPerformed;

    private void Awake()
    {
        if(_instance != null && _instance != this)
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
    }

    private void OnDisable()
    {

        controls.Pirata.Jump.performed -= ctx => OnJumpPerformed?.Invoke();
        controls.Pirata.Interact.performed -= ctx => OnInteractPerformed?.Invoke();

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


}
