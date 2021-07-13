using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState

{

    // Input
    private int xInput;
    private bool jumpInput;
    private bool jumpInputStop;
    private bool grabInput;
    private bool dashInput;

    // Check    
    private bool isGrounded;    
    private bool isTouchingWall;
    private bool isTouchingWallBack;
    private bool oldIsTouchingWall;
    private bool oldIsTouchingWallBack;
    private bool isTouchingLedge;
    private bool isJumping;
    
    private float startWallJumpCoyoteTime;
    private bool coyoteTime;
    private bool wallJumpCoyoteTime;
    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {

    }

    public override void DoChecks()
    {
        base.DoChecks();

        oldIsTouchingWall = isTouchingWall;
        oldIsTouchingWallBack = isTouchingWallBack;

        isGrounded = player.CheckIfGrounded();
        isTouchingWall = player.CheckIfTouchingWall();
        isTouchingWallBack = player.CheckIfTouchingWallback();
        isTouchingLedge = player.CheckIfTouchingLedge();

        if (isTouchingWall && !isTouchingLedge){
            player.LedgeClimbState.SetDetectedPosition(player.transform.position);
        }

        if (!wallJumpCoyoteTime && !isTouchingWall && !isTouchingWallBack  && (oldIsTouchingWallBack || oldIsTouchingWall)) {
            StartWallJumpCoyoteTime();
        }
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        oldIsTouchingWall = false;
        oldIsTouchingWallBack = false;
        isTouchingWallBack = false;
        isTouchingWall = false;
    }


    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CheckCoyoteTime();
        CheckWallJumpCoyoteTime();

        xInput = player.InputHandler.NormaInputX;
        jumpInput = player.InputHandler.JumpInput;
        jumpInputStop = player.InputHandler.JumpInputStop;
        grabInput = player.InputHandler.GrabInput;
        dashInput = player.InputHandler.DashInput;

        CheckJumpMultiplier();

        if (isGrounded && player.CurrentVelocity.y < 0.01f){
            stateMachine.ChangeState(player.LandState);
        } 
        else if (isTouchingWall && !isTouchingLedge) {
            stateMachine.ChangeState(player.LedgeClimbState);
        }
        else if (jumpInput && (isTouchingWall || isTouchingWallBack || wallJumpCoyoteTime)) {
            StopWallJumpCoyoteTime();
            coyoteTime = false;
            isTouchingWall = player.CheckIfTouchingWall();
            player.WallJumpState.DetermineWallJumpDirection(isTouchingWall);
            stateMachine.ChangeState(player.WallJumpState);
        }
        else if (jumpInput && player.JumpState.CanJump()){
            stateMachine.ChangeState(player.JumpState);
        }
        else if (grabInput && isTouchingWall && isTouchingLedge) {
            stateMachine.ChangeState(player.WallGrabState);
        }
        else if (isTouchingWall && xInput == player.FacingDirection && player.CurrentVelocity.y <= 0f)  {
            stateMachine.ChangeState(player.WallSlideState);
        }
        else if (dashInput && player.DashState.CheckIfCanDash()){
            stateMachine.ChangeState(player.DashState);
        }
        else {
            player.CheckIfShouldFlip(xInput);
            player.setVelocityX(playerData.movementVelocity * xInput);
            player.Anim.SetFloat("yVelocity", player.CurrentVelocity.y);
        }
    }


    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void CheckJumpMultiplier() {
        if (isJumping) {
            if (jumpInputStop){
                player.setVelocityY(player.CurrentVelocity.y * playerData.variableJumpHeightMultiplier);
                isJumping = false;
            }
            else if (player.CurrentVelocity.y <= 0f){
                isJumping = false;
            }
        }
    }
    private void CheckCoyoteTime()
    {
        if(coyoteTime && Time.time > startTime + playerData.coyoteTime) {
            coyoteTime = false;
            player.JumpState.DecreaseAmountOfJumpsLeft();
        }
    }
    private void CheckWallJumpCoyoteTime(){
        if (wallJumpCoyoteTime && Time.time > startWallJumpCoyoteTime + playerData.coyoteTime){
            wallJumpCoyoteTime = false;
        }
    }
    public void StartCoyoteTime() => coyoteTime = true;
    public void SetIsJumping() => isJumping = true;
    public void StartWallJumpCoyoteTime() {
        wallJumpCoyoteTime = true;
        startWallJumpCoyoteTime = Time.time;
    }
    public void StopWallJumpCoyoteTime() => wallJumpCoyoteTime = false;
} 
