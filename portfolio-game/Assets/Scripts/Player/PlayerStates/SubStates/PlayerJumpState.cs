public class PlayerJumpState : PlayerAbilityState
{
    private int _amountOfJumpsLeft;
    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {
        _amountOfJumpsLeft = playerData.amountOfJumps;
    }

    public override void Enter()
    {
        base.Enter();

        Player.InputHandler.UseJumpInput();

        Player.SfxPlayer.PlayOneShot(PlayerData.jumpClip);
        Player.SetVelocityY(PlayerData.jumpVelocity);
        IsAbilityDone = true;
        _amountOfJumpsLeft--;
        Player.InAirState.SetIsJumping();
    }

    public bool CanJump() => _amountOfJumpsLeft > 0;
    public void ResetAmountOfJumpsLeft() => _amountOfJumpsLeft = PlayerData.amountOfJumps;

    public void DecreaseAmountOfJumpsLeft () => _amountOfJumpsLeft--;  
}
