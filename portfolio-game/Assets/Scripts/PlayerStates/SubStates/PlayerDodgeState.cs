using UnityEngine;

public class PlayerDodgeState : PlayerAbilityState
{
    public PlayerDodgeState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        Player.SfxPlayer.PlayOneShot(PlayerData.dodgeClip);
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
    
        Player.SetVelocityX(PlayerData.dodgeSpeed * Player.FacingDirection);

        if (StartTime + PlayerData.dodgeTime <= Time.time) {
            IsAbilityDone = true;
            Player.InputHandler.UseDodgeInput();
            Player.SetVelocityX(0f);
        }
    }

}
