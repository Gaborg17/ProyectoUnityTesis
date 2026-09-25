using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }
    public override void CheckSwitchState()
    {
        if (Ctx.InputManager.MoveDirection().magnitude < 0.1f)
        {
            SwitchState(Factory.Idle());
        }
        if (Ctx.InputManager.isRunning() == false)
        {
            SwitchState(Factory.Walk());
        }



        if (Ctx.InputManager.AttackIsPressed())
        {
            SwitchState(Factory.InCombat());
        }
    }

    public override void EnterState()
    {
        Ctx.PAnimator.SetBool(Ctx.IsRunningHash, true);
        Ctx.isWalking = false;
        Ctx.ActualSpeed = Ctx.RunSpeed;
    }

    public override void ExitState()
    {
        Ctx.PAnimator.SetBool(Ctx.IsRunningHash, false);
    }

    public override void InitializeSubState()
    {
    }

    public override void UpdateState()
    {
        CheckSwitchState();

    }
}
