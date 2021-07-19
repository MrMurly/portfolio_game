public class PlayerHurtState : PlayerAbilityState
{
    public PlayerHurtState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Player.SfxPlayer.PlayOneShot(PlayerData.hurtClip);
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        IsAbilityDone = true;
    }
}
