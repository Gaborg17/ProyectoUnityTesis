public class EnemyGroundedState : EnemyBaseState
{
    public EnemyGroundedState(EnemyStateMachine currentContext, EnemyStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory)
    {
        IsRootState = true;

    }
    public override void CheckSwitchState()
    {

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
        if (Ctx.isWalking == true)
        {
            SetSubState(Factory.Walk());

        }
        else if (Ctx.isWalking == false)
        {
            SetSubState(Factory.Idle());
        }

    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }
}
