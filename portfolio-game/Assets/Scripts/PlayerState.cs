using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;
    protected bool isAnimationFinished;
    protected bool isExitingState;
    protected float startTime;
    private string animBoolName;

    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName ) {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        player.Anim.SetBool(animBoolName, true);
        DoChecks();
        startTime = Time.time;
        isAnimationFinished = false;
        isExitingState = false;
    }

    public virtual void Exit() {
        isExitingState = true;
        player.Anim.SetBool(animBoolName, false);
    }
    public virtual void LogicUpdate() {

    }
    public virtual void PhysicsUpdate(){
        DoChecks();
    }
    public virtual void DoChecks(){

    }

    public virtual void AnimationTrigger() {

    }

    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;
}
