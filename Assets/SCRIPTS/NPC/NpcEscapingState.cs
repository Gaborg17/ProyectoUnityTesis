using UnityEngine;
using UnityEngine.AI;

public class NpcEscapingState : NpcBaseState
{
    public NpcEscapingState(NpcStateMachine currentContext, NpcStateFactory npcStateFactory)
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
    }

    public override void EnterState()
    {
        Ctx.isWalking = false;
        Debug.Log("Entering Escape State");
        Escape();
        AgentModifiers();
        //Ctx.NpcAnimator.SetBool("IsWalking", true);
    }

    public override void ExitState()
    {
        Ctx.Agent.speed = 1f;
        Ctx.Agent.isStopped = true;

        Ctx.Escaping = false;
    }

    public override void InitializeSubState()
    {
    }

    public override void UpdateState()
    {
        //if (Vector3.Distance(Ctx.transform.position, Ctx.Player.position) < 10f)
        //{
        //    Escape();
        //}
        CheckSwitchState();
    }

    private void Escape()
    {
        Vector3 target = Ctx.Player.position + (Ctx.Player.forward * 5);
        Ctx.Agent.SetDestination(target);
    }
    private void AgentModifiers()
    {
        Ctx.Agent.isStopped = false;
        Ctx.Agent.stoppingDistance = 0;
        Ctx.Agent.speed = 3.5f;
    }
}
