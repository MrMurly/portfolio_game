using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="new player data", menuName = "data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Move State")]
    public float movementVelocity = 10f;
    [Header("Jump State")]
    public float jumpVelocity = 15f;
    public int amountOfJumps = 1;
    [Header("inAirState")]
    public float coyoteTime = 0.2f;
    public float variableJumpHeightMultiplier = 0.5f;
    [Header("Wall slide state")]
    public float wallSlideVelocity = 3f;
    [Header("WallJumpState")]
    public float wallJumpVelocity = 20f;
    public float wallJumpTime = 0.4f;
    [Header("LedgeClimbState")]
    public Vector2 startOffset;
    public Vector2 stopOffset;
    public Vector2 wallJumpAngle = new Vector2(1, 2);
    [Header("CheckVariables")]
    public float groundCheckRadius = 0.3f;
    public float wallCheckDistance = 0.4f;
    public LayerMask whatIsGround;
}
