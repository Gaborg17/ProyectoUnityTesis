
public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }
    public override void CheckSwitchState()
    {
        if (Ctx.InputManager.MoveDirection().magnitude > 0.01f)
        {
            if (Ctx.InputManager.isRunning() == true)
            {
                SwitchState(Factory.Run());
            }
            else
            {
                SwitchState(Factory.Walk());
            }
        }
        if (Ctx.InputManager.AttackIsPressed())
        {
            SwitchState(Factory.InCombat());
        }
    }

    public override void EnterState()
    {
        Ctx.PAnimator.SetBool(Ctx.IsWalkingHash, false);
        Ctx.PAnimator.SetBool(Ctx.IsRunningHash, false);
        Ctx.isWalking = false;
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
