using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="new player data", menuName = "data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Move State")]
    public float movementVelocity = 10f;
    

    [Header("Jump State")]
    public Transform jumpDust;
    public Transform landingDust;
    public float jumpVelocity = 15f;
    public int amountOfJumps = 1;

    [Header("inAirState")]
    public float coyoteTime = 0.2f;
    public float variableJumpHeightMultiplier = 0.5f;
    
    [Header("Wall slide state")]
    public Transform wallSlideDust;
    public float wallSlideVelocity = 3f;

    [Header("WallJumpState")]
    public Transform wallJumpDust;
    public float wallJumpVelocity = 20f;
    public float wallJumpTime = 0.4f;

    [Header("LedgeClimbState")]
    public Vector2 startOffset;
    public Vector2 stopOffset;
    public Vector2 wallJumpAngle = new Vector2(1, 2);
    
    [Header("DashState")]
    public float dashCooldown = 0.5f;
    public float maxHoldTime = 1f;
    public float holdTimeScale = 0.25f;
    public float dashTime = 0.2f;
    public float dashVelocity = 30f;
    public float drag = 10f;
    public float dashEndYMultiplier = 0.2f;
    public float distanceBetweenAfterImage = 0.5f;

    [Header("CheckVariables")]
    public float groundCheckRadius = 0.3f;
    public float wallCheckDistance = 0.4f;
    public LayerMask whatIsGround;
}
