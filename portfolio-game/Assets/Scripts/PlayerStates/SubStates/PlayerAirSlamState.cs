using UnityEngine;

public class PlayerAirSlamState : PlayerAbilityState
{
    private static readonly int YVelocity = Animator.StringToHash("yVelocity");

    public PlayerAirSlamState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Player.InputHandler.UseAttackInput();
        Player.SetVelocityY(-PlayerData.airSlamVelocity);
    } 


    public override void LogicUpdate() {
        base.LogicUpdate();
        Player.Anim.SetFloat(YVelocity, Player.CurrentVelocity.y);

        if (IsGrounded) {
            Player.StateMachine.ChangeState(Player.AirSlamLandState);
        }        
    }
}
