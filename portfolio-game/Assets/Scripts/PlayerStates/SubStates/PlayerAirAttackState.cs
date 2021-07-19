public class PlayerAirAttackState : PlayerAbilityState
{
    public PlayerAirAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Player.SfxPlayer.PlayOneShot(PlayerData.attackClip, 0.7f);
        
        Player.InputHandler.UseAttackInput();
        Player.SetVelocityY(PlayerData.airAttackBounce);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (IsAnimationFinished) {
            IsAbilityDone = true;
        }
    }
}
