using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState

{
    private int xInput;
    private bool isGrounded;    
    private bool isTouchingWall;
    private bool isTouchingWallBack;
    private bool isJumping;
    private bool jumpInput;
    private bool coyoteTime;
    private bool jumpInputStop;
    private bool grabInput;
    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {

    }

    public override void DoChecks()
    {
        base.DoChecks();
        isGrounded = player.CheckIfGrounded();
        isTouchingWall = player.CheckIfTouchingWall();
        isTouchingWallBack = player.CheckIfTouchingWallback();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }


    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CheckCoyoteTime();

        xInput = player.InputHandler.NormaInputX;
        jumpInput = player.InputHandler.JumpInput;
        jumpInputStop = player.InputHandler.JumpInputStop;
        grabInput = player.InputHandler.GrabInput;
        
        CheckJumpMultiplier();

        if (isGrounded && player.CurrentVelocity.y < 0.01f){
            stateMachine.ChangeState(player.LandState);
        } 
        else if (jumpInput && (isTouchingWall || isTouchingWallBack)) {
            isTouchingWall = player.CheckIfTouchingWall();
            player.WallJumpState.DetermineWallJumpDirection(isTouchingWall);
            stateMachine.ChangeState(player.WallJumpState);
        }
        else if (jumpInput && player.JumpState.CanJump()){
            stateMachine.ChangeState(player.JumpState);
        }
        else if (grabInput && isTouchingWall) {
            stateMachine.ChangeState(player.WallGrabState);
        }
        else if (isTouchingWall && xInput == player.FacingDirection && player.CurrentVelocity.y <= 0f)  {
            stateMachine.ChangeState(player.WallSlideState);
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
    public void StartCoyoteTime() => coyoteTime = true;
    public void SetIsJumping() => isJumping = true;
} 
