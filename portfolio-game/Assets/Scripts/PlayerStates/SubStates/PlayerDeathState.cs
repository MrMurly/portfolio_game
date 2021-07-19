using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : PlayerAbilityState
{
    public PlayerDeathState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Player.SfxPlayer.PlayOneShot(PlayerData.deathClip);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Player.SetVelocityX(0f);
    }
}
