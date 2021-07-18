using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region StateVariables
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState {get; private set;}
    public PlayerMoveState MoveState {get; private set;}
    public PlayerJumpState JumpState {get; private set;}
    public PlayerInAirState InAirState {get; private set;}
    public PlayerLandState LandState {get; private set;}
    public PlayerWallSlideState WallSlideState {get; private set;}
    public PlayerWallGrabState WallGrabState {get; private set;}
    public PlayerWallJumpState WallJumpState {get; private set;}
    public PlayerLedgeClimbState LedgeClimbState {get; private set;}
    public PlayerDashState DashState {get; private set;}
    public PlayerAttackState GroundedAttackState {get; private set;}
    public PlayerAirSlamState AirSlamState {get; private set;}
    public PlayerAirSlamLandState AirSlamLandState {get; private set;}
    public PlayerAirAttackState AirAttackState {get; private set;}
    public PlayerAttackState GroundedAttackUpState {get; private set;}
    public PlayerAirAttackState AirAttackUpState {get; private set;}
    public PlayerDodgeState DodgeState {get; private set;}
    [SerializeField] PlayerData playerData;

    #endregion

    #region Components
    public Animator Anim {get; private set;}
    public PlayerInputHandler InputHandler {get; private set;}
    public Rigidbody2D RB {get; private set;}
    public Transform DashDirectionIndicator {get; private set;}
    public AudioSource sfxPlayer {get; private set;}
    #endregion
    
    #region Check Transform 
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform ledgeCheck;
    [SerializeField] private Transform wallCheck;
    #endregion

    #region OtherVariables
    private SpriteRenderer sprite;
    public int FacingDirection {get; private set;}
    public Vector2 CurrentVelocity {get; private set;}
    private Vector2 workspace;
    #endregion

    #region UnityCallbackFunc
    private void Awake() {
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");
        InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
        LandState = new PlayerLandState(this, StateMachine, playerData, "land");
        WallGrabState = new PlayerWallGrabState(this, StateMachine, playerData, "wallSlide");
        WallSlideState = new PlayerWallSlideState(this, StateMachine, playerData, "wallSlide");
        WallJumpState = new PlayerWallJumpState(this, StateMachine, playerData, "inAir");
        LedgeClimbState = new PlayerLedgeClimbState(this, StateMachine, playerData, "ledgeClimbState");
        DashState = new PlayerDashState(this, StateMachine, playerData, "inAir");
        GroundedAttackState = new PlayerAttackState(this, StateMachine, playerData, "attack");
        GroundedAttackUpState = new PlayerAttackState(this, StateMachine, playerData, "attackUp");
        AirSlamState = new PlayerAirSlamState(this, StateMachine, playerData, "airSlam");
        AirSlamLandState = new PlayerAirSlamLandState(this, StateMachine, playerData, "slamLand");
        AirAttackState = new PlayerAirAttackState(this, StateMachine, playerData, "airAttack");
        AirAttackUpState = new PlayerAirAttackState(this, StateMachine, playerData, "airAttackUp");
        DodgeState = new PlayerDodgeState(this, StateMachine, playerData, "dodge");
    }       

    private void Start() {
        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        RB = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        sfxPlayer = GetComponent<AudioSource>();
        FacingDirection = 1;
        DashDirectionIndicator = transform.Find("DashDirectionIndicator");

        StateMachine.Initialize(IdleState);
    }
    private void Update() {
        CurrentVelocity = RB.velocity;
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate() {
        StateMachine.CurrentState.PhysicsUpdate();
    }
    
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        string _stateText = StateMachine.CurrentState.ToString();
    
        GUIStyle customStyle = new GUIStyle();
        customStyle.fontSize = 30;   // can also use e.g. <size=30> in Rich Text
        customStyle.richText = true;
        Vector3 textPosition = transform.position + (Vector3.up * 0.3f);
        string richText = "<color=red><B>" + _stateText + "</B></color>";

        UnityEditor.Handles.Label(textPosition, richText, customStyle);
    }
    #endregion

    #region SetFunctions
    public void setVelocityZero() {
        RB.velocity = Vector2.zero;
        CurrentVelocity = Vector2.zero;
    }
    public void setVelocity(float velocity, Vector2 angle, int direction) {
        angle.Normalize();
        workspace.Set(angle.x * velocity * direction, angle.y * velocity);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }
    public void setVelocity(float velocity, Vector2 direction) {
        workspace = direction * velocity;
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }
    public void setVelocityX(float velocity) {
        workspace.Set(velocity, CurrentVelocity.y);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }
    public void setVelocityY(float velocity) {
        workspace.Set(CurrentVelocity.x, velocity);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }
    #endregion

    #region CheckFunction
    public bool CheckIfGrounded(){
        return Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
    }
    public bool CheckIfTouchingWall() {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }
    public bool CheckIfTouchingLedge() {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }
    public bool CheckIfTouchingWallback() {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * -FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }
    public void CheckIfShouldFlip(int xInput) {
        if (xInput != 0 && xInput != FacingDirection){
            Flip();
        }
    }

    #endregion

    #region otherFunctions
    public Vector2 DetermineCornerPosition() {
        RaycastHit2D xHit = Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
        float xDistance = xHit.distance;
        workspace.Set(xDistance * FacingDirection, 0f);
        RaycastHit2D yHit = Physics2D.Raycast(ledgeCheck.position + (Vector3)(workspace), Vector2.down, ledgeCheck.position.y - wallCheck.position.y, playerData.whatIsGround);
        float yDistance = yHit.distance;

        workspace.Set(wallCheck.position.x + (xDistance * FacingDirection), ledgeCheck.position.y + (yDistance * FacingDirection));
        return workspace;
    }
    private void AnimationTriggerFunction() => StateMachine.CurrentState.AnimationTrigger();

    private void AnimationFinishedTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
    private void Flip() {
        FacingDirection *= -1;
        sprite.flipX = !sprite.flipX;
    }
    
    #endregion
}


