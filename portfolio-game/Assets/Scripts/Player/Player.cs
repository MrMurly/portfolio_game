using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour, IDamage
{
    #region StateVariables
    //TODO: Move this into a dictionary
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
    public PlayerDeathState DeathState { get; private set; }
    public PlayerHurtState HurtState { get; private set; }
    [SerializeField] private PlayerData playerData;

    #endregion

    #region Components
    public Animator Anim {get; private set;}
    public PlayerInputHandler InputHandler {get; private set;}
    public Rigidbody2D Rb {get; private set;}
    public Transform DashDirectionIndicator {get; private set;}
    public AudioSource SfxPlayer {get; private set;}
    private SpriteRenderer _sprite;

    #endregion

    #region Check Transform

    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform ledgeCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform attackBox;
    private BoxCollider2D _attackBoxCollider;

    #endregion

    #region OtherVariables

    public Vector2 CurrentVelocity {get; private set;}
    private Vector2 _workspace;
    public int FacingDirection {get; private set;}

    private float _health;

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
        DeathState = new PlayerDeathState(this, StateMachine, playerData, "dead");
        HurtState = new PlayerHurtState(this, StateMachine, playerData, "hurt");
        DodgeState = new PlayerDodgeState(this, StateMachine, playerData, "dodge");
    }       

    private void Start() {
        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        Rb = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        SfxPlayer = GetComponent<AudioSource>();
        _attackBoxCollider = attackBox.GetComponent<BoxCollider2D>();

        FacingDirection = 1;
        DashDirectionIndicator = transform.Find("DashDirectionIndicator");
        _health = playerData.maxHealth;
        StateMachine.Initialize(IdleState);
    }
    private void Update() {
        CurrentVelocity = Rb.velocity;
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate() {
        StateMachine.CurrentState.PhysicsUpdate();
    }
    
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        var stateText = StateMachine.CurrentState.ToString();

        var customStyle = new GUIStyle {fontSize = 30, richText = true};
        var textPosition = transform.position + (Vector3.up * 0.3f);
        var richText = $"<color=red><B>{stateText}</B></color>";

        UnityEditor.Handles.Label(textPosition, richText, customStyle);
    }
    #endregion

    #region SetFunctions
    public void SetVelocityZero() {
        Rb.velocity = Vector2.zero;
        CurrentVelocity = Vector2.zero;
    }
    public void SetVelocity(float velocity, Vector2 angle, int direction) {
        angle.Normalize();
        _workspace.Set(angle.x * velocity * direction, angle.y * velocity);
        Rb.velocity = _workspace;
        CurrentVelocity = _workspace;
    }
    public void SetVelocity(float velocity, Vector2 direction) {
        _workspace = direction * velocity;
        Rb.velocity = _workspace;
        CurrentVelocity = _workspace;
    }
    public void SetVelocityX(float velocity) {
        _workspace.Set(velocity, CurrentVelocity.y);
        Rb.velocity = _workspace;
        CurrentVelocity = _workspace;
    }
    public void SetVelocityY(float velocity) {
        _workspace.Set(CurrentVelocity.x, velocity);
        Rb.velocity = _workspace;
        CurrentVelocity = _workspace;
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
    public bool CheckIfTouchingWallBack() {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * -FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }
    public void CheckIfShouldFlip(int xInput) {
        if (xInput != 0 && xInput != FacingDirection){
            Flip();
        }
    }

    public void CheckIfAttackHit(float damage, float knockback)
    {
        var attackHit = Physics2D.BoxCast(attackBox.position, _attackBoxCollider.size, 0f, Vector2.zero,
            Mathf.Infinity, playerData.whatIsEnemy);
        if (attackHit)
        {
            attackHit.transform.GetComponent<IDamage>()?.TakeDamage(damage, knockback);
        }
    }

    #endregion

    #region otherFunctions
    public Vector2 DetermineCornerPosition() {
        var wallCheckPosition = wallCheck.position;
        var ledgeCheckPosition = ledgeCheck.position;
        
        var xHit = Physics2D.Raycast(wallCheckPosition, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
        var xDistance = xHit.distance;
        _workspace.Set(xDistance * FacingDirection, 0f);

        var yHit = Physics2D.Raycast(ledgeCheckPosition + (Vector3)(_workspace), Vector2.down, ledgeCheckPosition.y - wallCheckPosition.y, playerData.whatIsGround);
        var yDistance = yHit.distance;
        
        _workspace.Set(wallCheckPosition.x + (xDistance * FacingDirection), ledgeCheckPosition.y + (yDistance * FacingDirection));
        return _workspace;
    }
    private void AnimationTriggerFunction() => StateMachine.CurrentState.AnimationTrigger();

    private void AnimationFinishedTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
    private void Flip() {
        FacingDirection *= -1;
        transform.Rotate(Vector3.up, 180f);
    }
    
    #endregion
    
    #region damage
    public void TakeDamage(float damage, float knockback)
    {
        _health -= damage;
        SetVelocityX(knockback);
        if (_health <= 0)
        {
            StateMachine.ChangeState(DeathState);
        }
        else
        {
            StateMachine.ChangeState(HurtState);
        }
    }

    public void Heal(float healAmount)
    {
        throw new System.NotImplementedException();
    }
    #endregion
}


