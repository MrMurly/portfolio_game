public class PlayerAbilityState : PlayerState
{
    protected bool IsAbilityDone;

    protected bool IsGrounded;

    protected PlayerAbilityState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) { 

    }

    protected override void DoChecks()
    {
        base.DoChecks();

        IsGrounded = Player.CheckIfGrounded();
    }

    public override void Enter()
    {
        base.Enter();

        IsAbilityDone = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!IsAbilityDone) return;
        
        if (IsGrounded && Player.CurrentVelocity.y < 0.01f) {
            StateMachine.ChangeState(Player.IdleState);
        }
        else{
            StateMachine.ChangeState(Player.InAirState);
        }
    }
}
