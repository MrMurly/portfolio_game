public class PlayerAirAttackState : PlayerAbilityState
{

    public PlayerAirAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Player.SetVelocityY(PlayerData.airAttackBounce);
        Player.SfxPlayer.PlayOneShot(PlayerData.attackClip, 0.7f);

    }
    
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        IsAbilityDone = true;
    }
}
