using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirSlamState : PlayerAbilityState
{

    public PlayerAirSlamState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.InputHandler.UseAttackInput();
        player.setVelocityY(-playerData.airSlamVelocity);
    } 


    public override void LogicUpdate() {
        base.LogicUpdate();
        player.Anim.SetFloat("yVelocity", player.CurrentVelocity.y);

        if (isGrounded) {
            player.StateMachine.ChangeState(player.AirSlamLandState);
        }        
    }
}
