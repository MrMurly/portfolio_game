public class PlayerTouchingWallState : PlayerState
{
    private int _xInput;
    private bool _isGrounded;
    private bool _isTouchingWall;
    private bool _jumpInput;
    private bool _isTouchingLedge;
    
    protected int YInput;
    protected bool GrabInput;

    protected PlayerTouchingWallState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    
    protected override void DoChecks()
    {
        base.DoChecks();
        _isGrounded = Player.CheckIfGrounded();
        _isTouchingWall = Player.CheckIfTouchingWall();
        _isTouchingLedge = Player.CheckIfTouchingLedge();

        if (_isTouchingLedge && !_isTouchingLedge) {
            Player.LedgeClimbState.SetDetectedPosition(Player.transform.position);
        }
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        _xInput = Player.InputHandler.NormaInputX;
        _jumpInput = Player.InputHandler.JumpInput;
        YInput = Player.InputHandler.NormaInputY;
        GrabInput = Player.InputHandler.GrabInput;

        if (_jumpInput){
            Player.WallJumpState.DetermineWallJumpDirection(_isTouchingWall);
            StateMachine.ChangeState(Player.JumpState);
        }
        else if (_isGrounded && !GrabInput && !_isGrounded) {
            StateMachine.ChangeState(Player.IdleState);
        }
        else if (!_isTouchingWall || (_xInput != Player.FacingDirection && !GrabInput))
        {
            StateMachine.ChangeState(Player.InAirState);
        }
        else if (_isTouchingWall && !_isTouchingLedge) {
            StateMachine.ChangeState(Player.LedgeClimbState);
        }
    }
}
