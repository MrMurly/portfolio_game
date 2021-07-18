using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeState : PlayerAbilityState
{
    public PlayerDodgeState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        player.sfxPlayer.PlayOneShot(playerData.dodgeClip);
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
    
        player.setVelocityX(playerData.dodgeSpeed * player.FacingDirection);

        if (startTime + playerData.dodgeTime <= Time.time) {
            isAbilityDone = true;
            player.InputHandler.UseDodgeInput();
            player.setVelocityX(0f);
        }
    }

}
