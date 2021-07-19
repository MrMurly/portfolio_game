public class PlayerAirSlamLandState : PlayerGroundedState
{
    public PlayerAirSlamLandState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {

    }

    public override void Enter()
    {
        base.Enter();

        Player.SfxPlayer.PlayOneShot(PlayerData.airSlamClip, 0.5f);
        Player.SetVelocityZero();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!IsExitingState && IsAnimationFinished) {
            StateMachine.ChangeState(Player.IdleState);
        }
    }
}
