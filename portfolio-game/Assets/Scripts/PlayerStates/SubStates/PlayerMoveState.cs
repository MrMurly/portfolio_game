using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {

    }
    

    public override void AnimationTrigger() {
        var footstepAudioClip = PlayerData.footstepClips[Random.Range(0, PlayerData.footstepClips.Length)];
        Player.SfxPlayer.PlayOneShot(footstepAudioClip);
    }
    public override void LogicUpdate(){
        base.LogicUpdate();

        Player.CheckIfShouldFlip(XInput);
        
        Player.SetVelocityX(PlayerData.movementVelocity * XInput);

        if (XInput == 0 && !IsExitingState) {
            StateMachine.ChangeState(Player.IdleState);
        }
    }
    
}
