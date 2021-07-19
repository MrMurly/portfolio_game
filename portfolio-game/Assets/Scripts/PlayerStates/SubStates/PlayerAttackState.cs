using UnityEngine;

public class PlayerAttackState : PlayerAbilityState
{
    private static readonly int AttackRelease = Animator.StringToHash("attackRelease");
    private bool _attackInput;
    private bool _attacked;

    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _attacked = false;
        Player.SfxPlayer.PlayOneShot(PlayerData.attackClip, 0.7f);
    }
    
    public override void LogicUpdate() {
        base.LogicUpdate();
        
        Player.SetVelocityX(0f);
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        IsAbilityDone = true;
    }
}
