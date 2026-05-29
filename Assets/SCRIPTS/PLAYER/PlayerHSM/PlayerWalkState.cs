using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) 
        : base(currentContext, playerStateFactory) { }
    
    public override void CheckSwitchState()
    {
        if (Ctx.InputManager.MoveDirection().magnitude < 0.1f)
        {
            SwitchState(Factory.Idle());
        }

    }

    public override void EnterState()
    {
        //Animacion walk = true
        Ctx.isWalking = true;
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


}
