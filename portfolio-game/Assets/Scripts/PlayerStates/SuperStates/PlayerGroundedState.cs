using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{

    protected int xInput;
    private int yInput;
    protected bool JumpInput;
    private bool grabInput;
    private bool dashInput;
    private bool attackInput;
    private bool dodgeInput;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isTouchingLedge;
   public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {

   }

    public override void DoChecks(){
       base.DoChecks();
       isGrounded = player.CheckIfGrounded();
       isTouchingWall = player.CheckIfTouchingWall();
       isTouchingLedge = player.CheckIfTouchingLedge();
    }

    public override void Enter(){
        base.Enter();
        player.JumpState.ResetAmountOfJumpsLeft();
        player.DashState.ResetCanDash();

    }
    public override void Exit(){
       base.Exit();
    }
    public override void LogicUpdate(){
       base.LogicUpdate();

        xInput = player.InputHandler.NormaInputX;
        yInput = player.InputHandler.NormaInputY;
        JumpInput = player.InputHandler.JumpInput;
        grabInput = player.InputHandler.GrabInput;
        dashInput = player.InputHandler.DashInput;
        attackInput = player.InputHandler.AttackInput;
        dodgeInput = player.InputHandler.DodgeInput;
        
        
        if (JumpInput && player.JumpState.CanJump()) {
            stateMachine.ChangeState(player.JumpState);
        } 
        else if (!isGrounded) {
            player.InAirState.StartCoyoteTime();
            stateMachine.ChangeState(player.InAirState);
        } 
        else if (dodgeInput) {
            stateMachine.ChangeState(player.DodgeState);
        }
        else if (attackInput) {
            if (yInput > 0) {
                stateMachine.ChangeState(player.GroundedAttackUpState);
            }
            else {
                stateMachine.ChangeState(player.GroundedAttackState);
            }
        }
        else if (isTouchingWall && grabInput && isTouchingLedge) {
            stateMachine.ChangeState(player.WallSlideState);
        }
        else if (dashInput && player.DashState.CheckIfCanDash()){
            stateMachine.ChangeState(player.DashState);
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
 