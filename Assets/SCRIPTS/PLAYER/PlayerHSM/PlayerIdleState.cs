
public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }
    public override void CheckSwitchState()
    {
        if (Ctx.InputManager.MoveDirection().magnitude > 0.01f)
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
        //animacionWalk = false
        Ctx.PAnimator.SetBool(Ctx.IsWalkingHash, false);
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
