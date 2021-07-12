using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{

    protected int xInput;
    protected bool JumpInput;
    private bool isGrounded;
   public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {

   }

   public override void DoChecks(){
       base.DoChecks();
       isGrounded = player.CheckIfGrounded();
   }

   public override void Enter(){
       base.Enter();
       player.jumpState.ResetAmountOfJumpsLeft();
   }
    public override void Exit(){
       base.Exit();
    }
    public override void LogicUpdate(){
       base.LogicUpdate();

        xInput = player.InputHandler.NormaInputX;
        JumpInput = player.InputHandler.JumpInput;
        if (JumpInput && player.jumpState.CanJump()) {
            player.InputHandler.UseJumpInput();
            stateMachine.ChangeState(player.jumpState);
        } else if (!isGrounded) {
            player.inAirState.StartCoyoteTime();
            stateMachine.ChangeState(player.inAirState);
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
 