using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    public EnemyIdleState(EnemyStateMachine currentContext, EnemyStateFactory playerStateFactory)
: base(currentContext, playerStateFactory) { }
    public override void CheckSwitchState()
    {
        if(Ctx.Agent.isStopped == false)
        {
            SwitchState(Factory.Walk());
            
        }
    }

    public override void EnterState()
    {
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
