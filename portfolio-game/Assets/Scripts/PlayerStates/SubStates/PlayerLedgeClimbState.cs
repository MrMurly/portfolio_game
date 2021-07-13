using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeClimbState : PlayerState
{
    private Vector2 detectedPos;
    private Vector2 cornerPosition;
    private Vector2 startPosition;
    private Vector2 stopPosition;
    private bool isHanging;
    private bool isClimbing;
    private bool jumpInput;
    private int xInput;
    private int yInput;
    public PlayerLedgeClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Exit()
    {
        base.Exit();

        isHanging = false;

        if (isClimbing) {
            player.transform.position = stopPosition;
            isClimbing = false;
        }
    }
    public override void Enter()
    {
        base.Enter();

        player.setVelocityZero();
        player.transform.position = detectedPos;
        cornerPosition = player.DetermineCornerPosition();
        startPosition.Set(cornerPosition.x - (player.FacingDirection * playerData.startOffset.x), cornerPosition.y - playerData.startOffset.y);
        stopPosition.Set(cornerPosition.x + (player.FacingDirection * playerData.stopOffset.x), cornerPosition.y + playerData.stopOffset.y);

        player.transform.position = startPosition;
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationFinished) {
            stateMachine.ChangeState(player.IdleState);
            return;
        }

        xInput = player.InputHandler.NormaInputX;
        yInput = player.InputHandler.NormaInputY;
        jumpInput = player.InputHandler.JumpInput;

        player.setVelocityZero();
        player.transform.position = startPosition;

        if( xInput == player.FacingDirection && isHanging && !isClimbing) {
            isClimbing = true;
            player.Anim.SetBool("climbLedge", true);
        }
        else if (yInput == -1 && isHanging && !isClimbing) {
            stateMachine.ChangeState(player.InAirState);
        }
        else if (jumpInput && !isClimbing) {
            player.WallJumpState.DetermineWallJumpDirection(true);
            stateMachine.ChangeState(player.WallJumpState);
        }
    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        player.Anim.SetBool("climbLedge", false);
    }
    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        isHanging = true;
    }
    public void SetDetectedPosition(Vector2 position) => detectedPos = position;
}
