using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {

    }
    

    public override void AnimationTrigger() {
        AudioClip clipToPlay = playerData.footstepClips[Random.Range(0, playerData.footstepClips.Length)];
        player.sfxPlayer.PlayOneShot(clipToPlay);
    }
    public override void LogicUpdate(){
        base.LogicUpdate();

        player.CheckIfShouldFlip(xInput);
        
        player.setVelocityX(playerData.movementVelocity * xInput);

        if (xInput == 0 && !isExitingState) {
            stateMachine.ChangeState(player.IdleState);
        }
    }
    
}
