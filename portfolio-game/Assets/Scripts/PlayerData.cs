using UnityEngine;

[CreateAssetMenu(fileName ="new player data", menuName = "data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    public float maxHealth = 10f;
    public AudioClip hurtClip;
    public AudioClip deathClip;
    [Header("Move State")]
    public AudioClip[] footstepClips;

    public float movementVelocity = 10f;


    [Header("Jump State")]
    public AudioClip jumpClip;

    public float jumpVelocity = 15f;
    public int amountOfJumps = 1;


    [Header("landState")]
    public AudioClip landingClip;

    [Header("inAirState")]
    public float coyoteTime = 0.2f;

    public float variableJumpHeightMultiplier = 0.5f;


    [Header("Wall slide state")]
    public AudioClip wallSlideClip;

    public float wallSlideVelocity = 3f;


    [Header("WallJumpState")]
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

    [Header("Attack")]
    public AudioClip airSlamClip;

    public AudioClip attackClip;
    public float airSlamVelocity = 10f;
    public float airAttackBounce = 1f;

    [Header("DodgeState")]
    public AudioClip dodgeClip;

    public float dodgeTime = 2f;
    public float dodgeSpeed = 20f;
    public float dodgeDeacceleration = 14f;


    [Header("CheckVariables")]
    public float groundCheckRadius = 0.3f;

    public float wallCheckDistance = 0.4f;
    public LayerMask whatIsGround;
}
