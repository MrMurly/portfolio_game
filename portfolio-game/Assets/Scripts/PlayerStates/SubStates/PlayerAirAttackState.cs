using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirAttackState : PlayerAbilityState
{
    public PlayerAirAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.InputHandler.UseAttackInput();

        player.setVelocityY(playerData.airAttackBounce);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationFinished) {
            isAbilityDone = true;
        }
    }
}
