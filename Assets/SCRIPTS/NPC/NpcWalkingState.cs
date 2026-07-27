using UnityEngine;

public class NpcWalkingState : NpcBaseState
{
    public NpcWalkingState(NpcStateMachine currentContext, NpcStateFactory npcStateFactory)
: base(currentContext, npcStateFactory)
    {
        IsRootState = true;

    }
    public override void CheckSwitchState()
    {
        if (!Ctx.Agent.pathPending && Ctx.Agent.remainingDistance <= Ctx.Agent.stoppingDistance)
        {
            SwitchState(Factory.Idle());
        }

        if(Ctx.Escaping == true)
        {
            SwitchState(Factory.IsEscaping());
        }
    }

    public override void EnterState()
    {

        AgentModifiers();
        //Ctx.NpcAnimator.SetBool("IsWalking", true);
        Ctx.isWalking = true;
        Ctx.Escaping = false;
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
