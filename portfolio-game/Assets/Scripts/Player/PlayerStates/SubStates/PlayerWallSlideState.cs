public class PlayerWallSlideState : PlayerTouchingWallState
{
    public PlayerWallSlideState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (IsExitingState) return;
        
        Player.SetVelocityY(-PlayerData.wallSlideVelocity);

        if (GrabInput) {
            StateMachine.ChangeState(Player.WallGrabState);
        }
    }
}
