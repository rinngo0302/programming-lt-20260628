using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputAdapter : MonoBehaviour
{
    [SerializeField, Tooltip("操作対象のKartController")]
    KartController _kartController;

    [SerializeField, Tooltip("Input System Actionsアセット")]
    InputActionAsset _inputActions;

    InputAction _steerAction;
    InputAction _accelerateAction;
    InputAction _brakeAction;
    InputAction _useItemAction;

    void Awake()
    {
        InputActionMap playerMap = _inputActions.FindActionMap("Player");
        _steerAction = playerMap.FindAction("Steer");
        _accelerateAction = playerMap.FindAction("Accelerate");
        _brakeAction = playerMap.FindAction("Brake");
        _useItemAction = playerMap.FindAction("UseItem");
        playerMap.Enable();
    }

    void Update()
    {
        _kartController.Steer = _steerAction.ReadValue<float>();
        _kartController.Accelerate = _accelerateAction.IsPressed();
        _kartController.Brake = _brakeAction.IsPressed();

        if (_useItemAction.WasPressedThisFrame())
        {
            _kartController.UseItem();
        }
    }

    void OnDestroy()
    {
        _inputActions.FindActionMap("Player")?.Disable();
    }
}
