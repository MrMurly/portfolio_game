using UnityEngine;

public class PlayerAttackState : PlayerAbilityState
{
    private static readonly int AttackRelease = Animator.StringToHash("attackRelease");
    private bool _attackInput;

    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Player.SfxPlayer.PlayOneShot(PlayerData.attackClip, 0.7f);
    }
    
    public override void LogicUpdate() {
        base.LogicUpdate();
        
        Player.SetVelocityX(0f);
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        Player.CheckIfAttackHit(PlayerData.normalDamage, PlayerData.normalKnockback);
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        IsAbilityDone = true;
    }
}
