public class PlayerAttackState : PlayerAbilityState
{
    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void Enter() {
        base.Enter();

        Player.SfxPlayer.PlayOneShot(PlayerData.attackClip, 0.7f);
        Player.InputHandler.UseAttackInput();
    }
    public override void LogicUpdate() {
        base.LogicUpdate();
        Player.SetVelocityX(0f);
        if (IsAnimationFinished) {
            IsAbilityDone = true;
        }
    }
}
