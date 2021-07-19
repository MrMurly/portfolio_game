public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {

    }
    public override void Enter(){
        base.Enter();
        Player.SetVelocityX(0f);
    }
    public override void LogicUpdate(){
       base.LogicUpdate();

        if (XInput != 0 && !IsExitingState) {
           StateMachine.ChangeState(Player.MoveState);
        }
    }
}
