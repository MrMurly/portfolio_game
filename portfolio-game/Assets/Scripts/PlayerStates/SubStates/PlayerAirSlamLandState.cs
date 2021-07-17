using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirSlamLandState : PlayerGroundedState
{
    public PlayerAirSlamLandState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {

    }

    public override void Enter()
    {
        base.Enter();

        player.setVelocityZero();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!isExitingState && isAnimationFinished) {
            stateMachine.ChangeState(player.IdleState);
        }
    }
}
