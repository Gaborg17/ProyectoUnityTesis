using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) 
    { 
        IsRootState = true;
        
    }
    public override void CheckSwitchState()
    {
        if(Ctx.JumpRequested == true)
        {
            SwitchState(Factory.Jump());
        }
    }

    public override void EnterState()
    {
        InitializeSubState();
    }

    public override void ExitState()
    {
        
    }

    public override void InitializeSubState()
    {
        if(Ctx.InputManager == null)
        {
            SetSubState(Factory.Idle());
            return;
        }

        if(Ctx.InputManager.MoveDirection().magnitude < 0.01f)
        {
            SetSubState(Factory.Idle());
        }
        else if(Ctx.InputManager.MoveDirection().magnitude > 0.01f)
        {
            SetSubState(Factory.Walk());
        }

    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }


}
