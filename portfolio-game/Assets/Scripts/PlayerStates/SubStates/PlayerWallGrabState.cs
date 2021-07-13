using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallGrabState : PlayerTouchingWallState
{
    private Vector2 holdPosition;
    public PlayerWallGrabState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        holdPosition = player.transform.position;
        HoldPosition();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        HoldPosition();

        //tilt camera up
        if (!isExitingState && (yInput < 0 || !grabInput)) {
            stateMachine.ChangeState(player.WallSlideState);
        }
    }

    private void HoldPosition() {
        player.transform.position = holdPosition;
        player.setVelocityX(0f);
        player.setVelocityY(0f);
    }
}
