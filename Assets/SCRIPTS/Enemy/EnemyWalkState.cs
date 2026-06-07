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
        
        AgentModifiers();
        Ctx.EnemyAnimator.SetBool("IsWalking", true);
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
    private void AgentModifiers()
    {
        Ctx.Agent.isStopped = false;
        Ctx.Agent.stoppingDistance = 0;
    }
}
