using UnityEngine;

public class EnemyWalkState : EnemyBaseState
{
    public EnemyWalkState(EnemyStateMachine currentContext, EnemyStateFactory playerStateFactory)
: base(currentContext, playerStateFactory){}
    public override void CheckSwitchState()
    {
        if(!Ctx.Agent.pathPending && Ctx.Agent.remainingDistance <= Ctx.Agent.stoppingDistance)
        {
            SwitchState(Factory.Idle());            
        }
    }

    public override void EnterState()
    {
        Ctx.Agent.isStopped = false;
        Ctx.isWalking = true;
    }

    public override void ExitState()
    {
        Ctx.Agent.isStopped = true;
    }

    public override void InitializeSubState()
    {


    }

    public override void UpdateState()
    {
        Walk();
        CheckSwitchState();
    }

    private void Walk()
    {
        Ctx.Agent.SetDestination(Ctx.Destinos[0].position);
        
    }
}
