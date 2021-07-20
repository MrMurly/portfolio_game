using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallGrabState : PlayerTouchingWallState
{
    private Vector2 _holdPosition;
    public PlayerWallGrabState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        _holdPosition = Player.transform.position;
        HoldPosition();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        HoldPosition();

        //TODO: tilt camera up
        if (!IsExitingState && (YInput < 0 || !GrabInput)) {
            StateMachine.ChangeState(Player.WallSlideState);
        }
    }

    private void HoldPosition() {
        Player.transform.position = _holdPosition;
        Player.SetVelocityX(0f);
        Player.SetVelocityY(0f);
    }
}
