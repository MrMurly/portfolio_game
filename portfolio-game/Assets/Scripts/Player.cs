using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    // https://www.youtube.com/playlist?list=PLy78FINcVmjA0zDBhLuLNL1Jo6xNMMq-W
    #region StateVariables
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState idleState {get; private set;}
    public PlayerMoveState moveState {get; private set;}
    public PlayerJumpState jumpState {get; private set;}
    public PlayerInAirState inAirState {get; private set;}
    public PlayerLandState landState {get; private set;}
    [SerializeField] PlayerData playerData;

    #endregion

    #region Components
    public Animator Anim {get; private set;}
    public PlayerInputHandler InputHandler {get; private set;}
    public Rigidbody2D RB {get; private set;}
    #endregion
    #region Check Transform 
    [SerializeField] private Transform groundCheck;
    #endregion

    #region OtherVariables
    public Vector2 CurrentVelocity {get; private set;}
    public int FacingDirection {get; private set;}
    private Vector2 workspace;
    #endregion

    #region UnityCallbackFunc
    private void Awake() {
        StateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        moveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        jumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");
        inAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
        landState = new PlayerLandState(this, StateMachine, playerData, "land");
    }   

    private void Start() {
        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        RB = GetComponent<Rigidbody2D>();
        FacingDirection = 1;

        StateMachine.Initialize(idleState);
    }
    private void Update() {
        CurrentVelocity = RB.velocity;
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate() {
        StateMachine.CurrentState.PhysicsUpdate();
    }
    #endregion

    #region SetFunctions
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
    public void CheckIfShouldFlip(int xInput) {
        if (xInput != 0 && xInput != FacingDirection){
            Flip();
        }
    }

    #endregion

    #region otherFunctions
    private void AnimationTriggerFunction() => StateMachine.CurrentState.AnimationTrigger();

    private void AnimationFinishedTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
    private void Flip() {
        FacingDirection *= -1;
        transform.Rotate(0f, 180f, 0f);
    }
    #endregion
}


