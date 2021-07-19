using UnityEngine;

public class PlayerState
{
    private readonly string _animBoolName;
    
    protected readonly Player Player;
    protected readonly PlayerStateMachine StateMachine;
    protected readonly PlayerData PlayerData;
    protected bool IsAnimationFinished;
    protected bool IsExitingState;
    protected float StartTime;

    protected PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName ) {
        Player = player;
        StateMachine = stateMachine;
        PlayerData = playerData;
        _animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        Player.Anim.SetBool(_animBoolName, true);
        DoChecks();
        StartTime = Time.time;
        IsAnimationFinished = false;
        IsExitingState = false;
    }

    public virtual void Exit() {
        IsExitingState = true;
        Player.Anim.SetBool(_animBoolName, false);
    }
    public virtual void LogicUpdate() {

    }
    public virtual void PhysicsUpdate(){
        DoChecks();
    }

    protected virtual void DoChecks(){

    }

    public virtual void AnimationTrigger() {

    }

    public virtual void AnimationFinishTrigger() => IsAnimationFinished = true;
}
