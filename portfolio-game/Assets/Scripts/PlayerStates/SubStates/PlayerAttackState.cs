using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerAbilityState
{
    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    override public void Enter() {
        base.Enter();
        player.InputHandler.UseAttackInput();
    }
    override public void LogicUpdate() {
        base.LogicUpdate();
        player.setVelocityX(0f);
        if (isAnimationFinished) {
            isAbilityDone = true;
        }
    }
}
