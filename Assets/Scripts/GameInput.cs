using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour {
    public static GameInput Instance { get; private set; }

    private PlayerInputActions _playerInputActions;

    //public event EventHandler OnPlayerAttack;

    private void Awake() {
        Instance = this;

        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Enable();
        
        //_playerInputActions.Combat.Attack.started += PlayerAttack_started;
    }


    private void OnEnable()
    {
        if (_playerInputActions == null)
        {
            _playerInputActions = new PlayerInputActions();
        }

        _playerInputActions.Player.Enable();
        _playerInputActions.Combat.Enable();
    }



    public Vector2 GetMovementVector() {
        Vector2 inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();
        return inputVector;
    }

    public Vector3 GetMousePosition() {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        return mousePos;
    }

    public void EnableMovement()
    {
        _playerInputActions.Enable();

    }

    public void DisableMovement()
    {
        _playerInputActions.Disable();

    }

    public void OnDisable()
    {
        _playerInputActions.Player.Disable();
        _playerInputActions.Combat.Disable();
        _playerInputActions.Dispose();

    }

    //private void PlayerAttack_started(InputAction.CallbackContext obj)
    //{
    //    OnPlayerAttack?.Invoke(this, EventArgs.Empty);
    //}
}
