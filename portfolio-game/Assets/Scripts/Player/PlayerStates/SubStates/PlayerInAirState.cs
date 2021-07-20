using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{

    // Input
    private int _xInput;
    private int _yInput;
    private bool _jumpInput;
    private bool _jumpInputStop;
    private bool _grabInput;
    private bool _dashInput;
    private bool _attackInput;

    // Check    
    private bool _isGrounded;    
    private bool _isTouchingWall;
    private bool _isTouchingWallBack;
    private bool _oldIsTouchingWall;
    private bool _oldIsTouchingWallBack;
    private bool _isTouchingLedge;
    private bool _isJumping;
    
    private float _startWallJumpCoyoteTime;
    private bool _coyoteTime;
    private bool _wallJumpCoyoteTime;
    private static readonly int YVelocity = Animator.StringToHash("yVelocity");

    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {

    }

    protected override void DoChecks()
    {
        base.DoChecks();

        _oldIsTouchingWall = _isTouchingWall;
        _oldIsTouchingWallBack = _isTouchingWallBack;

        _isGrounded = Player.CheckIfGrounded();
        _isTouchingWall = Player.CheckIfTouchingWall();
        _isTouchingWallBack = Player.CheckIfTouchingWallBack();
        _isTouchingLedge = Player.CheckIfTouchingLedge();

        if (_isTouchingWall && !_isTouchingLedge){
            Player.LedgeClimbState.SetDetectedPosition(Player.transform.position);
        }

        if (!_wallJumpCoyoteTime && !_isTouchingWall && !_isTouchingWallBack  && (_oldIsTouchingWallBack || _oldIsTouchingWall)) {
            StartWallJumpCoyoteTime();
        }
    }
    public override void Exit()
    {
        base.Exit();

        _oldIsTouchingWall = false;
        _oldIsTouchingWallBack = false;
        _isTouchingWallBack = false;
        _isTouchingWall = false;
    }


    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CheckCoyoteTime();
        CheckWallJumpCoyoteTime();
        
        _xInput = Player.InputHandler.NormaInputX;
        _yInput = Player.InputHandler.NormaInputY;
        _jumpInput = Player.InputHandler.JumpInput;
        _jumpInputStop = Player.InputHandler.JumpInputStop;
        _grabInput = Player.InputHandler.GrabInput;
        _dashInput = Player.InputHandler.DashInput;
        _attackInput = Player.InputHandler.AttackInput;

        CheckJumpMultiplier();

        if (_isGrounded && Player.CurrentVelocity.y < 0.01f){
            StateMachine.ChangeState(Player.LandState);
        } 
        else if (_attackInput) {
            if (_yInput < 0f) {
                StateMachine.ChangeState(Player.AirSlamState);
            } 
            else if (_yInput > 0f) {
                StateMachine.ChangeState(Player.AirAttackUpState);
            }
            else {
                StateMachine.ChangeState(Player.AirAttackState);
            }
        }
        else if (_isTouchingWall && !_isTouchingLedge) {
            StateMachine.ChangeState(Player.LedgeClimbState);
        }
        else switch (_jumpInput)
        {
            case true when (_isTouchingWall || _isTouchingWallBack || _wallJumpCoyoteTime):
                StopWallJumpCoyoteTime();
                _coyoteTime = false;
                _isTouchingWall = Player.CheckIfTouchingWall();
                Player.WallJumpState.DetermineWallJumpDirection(_isTouchingWall);
                StateMachine.ChangeState(Player.WallJumpState);
                break;
            case true when Player.JumpState.CanJump():
                StateMachine.ChangeState(Player.JumpState);
                break;
            default:
            {
                if (_grabInput && _isTouchingWall && _isTouchingLedge) {
                    StateMachine.ChangeState(Player.WallGrabState);
                }
                else if (_isTouchingWall && _xInput == Player.FacingDirection && Player.CurrentVelocity.y <= 0f)  {
                    StateMachine.ChangeState(Player.WallSlideState);
                }
                else if (_dashInput && Player.DashState.CheckIfCanDash()){
                    StateMachine.ChangeState(Player.DashState);
                }
                else {
                    Player.CheckIfShouldFlip(_xInput);
                    Player.SetVelocityX(PlayerData.movementVelocity * _xInput);
                    Player.Anim.SetFloat(YVelocity, Player.CurrentVelocity.y);
                }

                break;
            }
        }
    }


    private void CheckJumpMultiplier()
    {
        if (!_isJumping) return;
        
        if (_jumpInputStop){
            Player.SetVelocityY(Player.CurrentVelocity.y * PlayerData.variableJumpHeightMultiplier);
            _isJumping = false;
        }
        else if (Player.CurrentVelocity.y <= 0f){
            _isJumping = false;
        }
    }
    private void CheckCoyoteTime()
    {
        if (!_coyoteTime || !(Time.time > StartTime + PlayerData.coyoteTime)) return;
        
        _coyoteTime = false;
        Player.JumpState.DecreaseAmountOfJumpsLeft();
    }
    private void CheckWallJumpCoyoteTime(){
        if (_wallJumpCoyoteTime && Time.time > _startWallJumpCoyoteTime + PlayerData.coyoteTime){
            _wallJumpCoyoteTime = false;
        }
    }
    public void StartCoyoteTime() => _coyoteTime = true;
    public void SetIsJumping() => _isJumping = true;

    private void StartWallJumpCoyoteTime() {
        _wallJumpCoyoteTime = true;
        _startWallJumpCoyoteTime = Time.time;
    }

    private void StopWallJumpCoyoteTime() => _wallJumpCoyoteTime = false;
} 
