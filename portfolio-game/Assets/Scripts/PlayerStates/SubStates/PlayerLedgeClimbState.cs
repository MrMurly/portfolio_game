using UnityEngine;

public class PlayerLedgeClimbState : PlayerState
{
    private static readonly int ClimbLedge = Animator.StringToHash("climbLedge");
    
    private Vector2 _detectedPos;
    private Vector2 _cornerPosition;
    private Vector2 _startPosition;
    private Vector2 _stopPosition;
    private bool _isHanging;
    private bool _isClimbing;
    private bool _jumpInput;
    private int _xInput;
    private int _yInput;

    public PlayerLedgeClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Exit()
    {
        base.Exit();

        _isHanging = false;

        if (_isClimbing) {
            Player.transform.position = _stopPosition;
            _isClimbing = false;
        }
    }
    public override void Enter()
    {
        base.Enter();
        
        //TODO: Refactor this!!
        Player.SetVelocityZero();
        Player.transform.position = _detectedPos;
        _cornerPosition = Player.DetermineCornerPosition();
        _startPosition.Set(_cornerPosition.x - (Player.FacingDirection * PlayerData.startOffset.x), _cornerPosition.y - PlayerData.startOffset.y);
        _stopPosition.Set(_cornerPosition.x + (Player.FacingDirection * PlayerData.stopOffset.x), _cornerPosition.y + PlayerData.stopOffset.y);

        Player.transform.position = _startPosition;
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (IsAnimationFinished) {
            StateMachine.ChangeState(Player.IdleState);
            return;
        }

        _xInput = Player.InputHandler.NormaInputX;
        _yInput = Player.InputHandler.NormaInputY;
        _jumpInput = Player.InputHandler.JumpInput;

        Player.SetVelocityZero();
        Player.transform.position = _startPosition;

        if( _xInput == Player.FacingDirection && _isHanging && !_isClimbing) {
            _isClimbing = true;
            Player.Anim.SetBool(ClimbLedge, true);
        }
        else if (_yInput == -1 && _isHanging && !_isClimbing) {
            StateMachine.ChangeState(Player.InAirState);
        }
        else if (_jumpInput && !_isClimbing) {
            Player.WallJumpState.DetermineWallJumpDirection(true);
            StateMachine.ChangeState(Player.WallJumpState);
        }
    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        Player.Anim.SetBool(ClimbLedge, false);
    }
    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        _isHanging = true;
    }
    public void SetDetectedPosition(Vector2 position) => _detectedPos = position;
}
