using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput _playerInput;
    private Camera _cam;

    private Vector2 RawMovementInput {get; set;}
    private Vector2 RawDashDirectionInput {get; set;}
    public Vector2Int DashDirectionInput {get; private set;}

    public int NormaInputX {get; private set;}
    public int NormaInputY {get; private set;}
    public bool JumpInput {get; private set;}
    public bool JumpInputStop {get; private set;}
    public bool GrabInput {get; private set;}
    public bool DashInput {get; private set;}
    public bool DashInputStop {get; private set;}
    public bool AttackInput {get; private set;}
    private float _dashInputStartTime;
    public bool DodgeInput {get; private set;}
    [SerializeField] private float inputHoldTime = 0.2f;
    private float _jumpInputStartTime;

    private void Start() {
        _playerInput = GetComponent<PlayerInput>();
        _cam = Camera.main;
    }

    private void Update() {
        CheckDashInputHoldTime();
        CheckJumpInputHoldTime();
    }

    public void OnMoveInput(InputAction.CallbackContext context){
        RawMovementInput = context.ReadValue<Vector2>();

        //TODO: Dead zone for controllers

        NormaInputX = (int)(RawMovementInput * Vector2.right).normalized.x;
        NormaInputY = (int)(RawMovementInput * Vector2.up).normalized.y;

    }
   
    public void OnJumpInput(InputAction.CallbackContext context) {
        if (context.started) {
            JumpInput = true;
            JumpInputStop = false;
            _jumpInputStartTime = Time.time;
        }
        if (context.canceled) {
            JumpInputStop = true;
        }
    }
    public void OnGrabInput(InputAction.CallbackContext context) {
        if (context.started) {
            GrabInput = true;
        }

        if (context.canceled) {
            GrabInput = false;
        }
    }
    public void OnDashInput(InputAction.CallbackContext context) {
        if (context.started) {
            DashInput = true;
            DashInputStop = false;
            _dashInputStartTime = Time.time;
        }
        if (context.canceled) {
            DashInputStop = true;
        }
    }

    public void OnAttackInput(InputAction.CallbackContext context) {
        if (context.started) {
            AttackInput = true;
        }
        if (context.canceled) {
            AttackInput = false;
        }
    }

    public void OnDashDirectionInput(InputAction.CallbackContext context) {
        RawDashDirectionInput = context.ReadValue<Vector2>();

        if (_playerInput.currentControlScheme == "Keyboard") {
            RawDashDirectionInput = _cam.ScreenToWorldPoint(RawDashDirectionInput) - transform.position;
        }

        DashDirectionInput = Vector2Int.RoundToInt(RawDashDirectionInput.normalized);
    }

    public void OnDodgeDirectionInput(InputAction.CallbackContext context) {
        if (context.performed){
            DodgeInput = true;
        } 

    }
    public void UseJumpInput() => JumpInput = false;
    public void UseDashInput() => DashInput = false;
    public void UseAttackInput() => AttackInput = false;
    public void UseDodgeInput() => DodgeInput = false;

    private void CheckJumpInputHoldTime(){
        if(Time.time >= _jumpInputStartTime + inputHoldTime ){
            JumpInput = false;
        }
    }
    private void CheckDashInputHoldTime() {
        if (Time.time >= _dashInputStartTime + inputHoldTime) {
            DashInput = false;
        }
    }
}
