using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) 
        : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
        
    }
    public override void CheckSwitchState()
    {
        if (Ctx.JumpRequested == false)
        {
            SwitchState(Factory.Grounded());
        }
    }

    public override void EnterState()
    {
        InitializeSubState();
        Jump();
    }

    public override void ExitState()
    {

    }

    public override void InitializeSubState()
    {
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }


    private void Jump()
    {
        Ctx.Rb.AddForce(Vector3.up * Ctx.JumpForce, ForceMode.Impulse);
        Ctx.JumpRequested = false;
        
    }
}
