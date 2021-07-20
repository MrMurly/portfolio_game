using UnityEngine;

public class PlayerWallJumpState : PlayerAbilityState
{
    private int _wallJumpDirection;
    private static readonly int YVelocity = Animator.StringToHash("yVelocity");

    public PlayerWallJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        Player.SfxPlayer.PlayOneShot(PlayerData.jumpClip);

        Player.InputHandler.UseJumpInput();
        Player.JumpState.ResetAmountOfJumpsLeft();
        Player.SetVelocity(PlayerData.wallJumpVelocity, PlayerData.wallJumpAngle, _wallJumpDirection);
        Player.CheckIfShouldFlip(_wallJumpDirection);
        Player.JumpState.DecreaseAmountOfJumpsLeft();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Player.Anim.SetFloat(YVelocity, Player.CurrentVelocity.y);

        if (Time.time >= StartTime + PlayerData.wallJumpTime) {
            IsAbilityDone = true;
        }
    }
    public void DetermineWallJumpDirection(bool isTouchingWall) {
        if (isTouchingWall) {
            _wallJumpDirection = -Player.FacingDirection;
        }
        else {
            _wallJumpDirection = Player.FacingDirection;
        }
    }
}
