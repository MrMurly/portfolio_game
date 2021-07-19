public class PlayerGroundedState : PlayerState
{
    private int _yInput;
    private bool _jumpInput;
    private bool _grabInput;
    private bool _dashInput;
    private bool _attackInput;
    private bool _dodgeInput;
    private bool _isGrounded;
    private bool _isTouchingWall;
    private bool _isTouchingLedge;
    
    protected int XInput;

    protected PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) {

   }

   protected override void DoChecks(){
       base.DoChecks();
       _isGrounded = Player.CheckIfGrounded();
       _isTouchingWall = Player.CheckIfTouchingWall();
       _isTouchingLedge = Player.CheckIfTouchingLedge();
    }

    public override void Enter(){
        base.Enter();
        Player.JumpState.ResetAmountOfJumpsLeft();
        Player.DashState.ResetCanDash();
    }

    public override void LogicUpdate(){
       base.LogicUpdate();

        XInput = Player.InputHandler.NormaInputX;
        _yInput = Player.InputHandler.NormaInputY;
        _jumpInput = Player.InputHandler.JumpInput;
        _grabInput = Player.InputHandler.GrabInput;
        _dashInput = Player.InputHandler.DashInput;
        _attackInput = Player.InputHandler.AttackInput;
        _dodgeInput = Player.InputHandler.DodgeInput;
        
        
        if (_jumpInput && Player.JumpState.CanJump()) {
            StateMachine.ChangeState(Player.JumpState);
        } 
        else if (!_isGrounded) {
            Player.InAirState.StartCoyoteTime();
            StateMachine.ChangeState(Player.InAirState);
        } 
        else if (_dodgeInput) {
            StateMachine.ChangeState(Player.DodgeState);
        }
        else if (_attackInput)
        {
            StateMachine.ChangeState(_yInput > 0 ? Player.GroundedAttackUpState : Player.GroundedAttackState);
        }
        else if (_isTouchingWall && _grabInput && _isTouchingLedge) {
            StateMachine.ChangeState(Player.WallSlideState);
        }
        else if (_dashInput && Player.DashState.CheckIfCanDash()){
            StateMachine.ChangeState(Player.DashState);
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
 