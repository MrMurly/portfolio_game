using UnityEngine;


//TODO: Add afterimages
//TODO: Add custom sprite for dash
//TODO: Add camera zooom - fwoosh!
//TODO: Add particles
public class PlayerDashState : PlayerAbilityState
{
    private bool _isHolding;
    private bool _dashInputStop;
    private float _lastDashTime;
    private Vector2 _dashDirection;
    private Vector2 _dashDirectionInput;

    private bool CanDash {get; set;}

    public PlayerDashState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        CanDash = false;
        Player.InputHandler.UseDashInput();
        
        _isHolding = true;
        _dashDirection = Vector2.right * Player.FacingDirection;

        Time.timeScale = PlayerData.holdTimeScale;
        StartTime = Time.unscaledTime;

        Player.DashDirectionIndicator.gameObject.SetActive(true);
    }
    public override void Exit()
    {
        base.Exit();

        if (Player.CurrentVelocity.y > 0) {
            Player.SetVelocityY(Player.CurrentVelocity.y * PlayerData.dashEndYMultiplier);
        }

    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();


        if (!IsExitingState) { 
            if (_isHolding) {
                _dashDirection = Player.InputHandler.DashDirectionInput;
                _dashInputStop = Player.InputHandler.DashInputStop;

                if (_dashDirectionInput != Vector2.zero) {
                    _dashDirection = _dashDirectionInput;
                    _dashDirection.Normalize();
                }

                var angle = Vector2.SignedAngle(Vector2.right, _dashDirection);
                Player.DashDirectionIndicator.rotation = Quaternion.Euler(0f, 0f, angle - 45f);

                if (!_dashInputStop && !(Time.unscaledTime >= StartTime + PlayerData.maxHoldTime)) return;
                
                _isHolding = false;
                Time.timeScale = 1f;
                StartTime = Time.time;
                Player.CheckIfShouldFlip(Mathf.RoundToInt(_dashDirection.x));
                Player.Rb.drag = PlayerData.drag;
                Player.SetVelocity(PlayerData.dashVelocity, _dashDirection);
                Player.DashDirectionIndicator.gameObject.SetActive(false);
            }
            else {
                Player.SetVelocity(PlayerData.dashVelocity, _dashDirection);

                if (!(Time.time >= StartTime + PlayerData.dashTime)) return;
                
                Player.Rb.drag = 0f;
                IsAbilityDone = true;
                _lastDashTime = Time.time;
            }
        }
    }
    public bool CheckIfCanDash(){ 
        return CanDash && Time.time >= _lastDashTime + PlayerData.dashCooldown;
    }
    public void ResetCanDash() => CanDash = true;
}
