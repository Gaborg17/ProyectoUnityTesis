using UnityEngine;

public class AllyRetreatState : AllyBaseState
{
    public AllyRetreatState(AllyStateMachine currentContext, AllyStateFactory allyStateFactory)
: base(currentContext, allyStateFactory){}
    public override void CheckSwitchState()
    {
        if (!Ctx.Agent.pathPending && Ctx.Agent.remainingDistance <= Ctx.Agent.stoppingDistance)
        {
            SwitchState(Factory.Chase());
        }
    }

    public override void EnterState()
    {
        Ctx.AllyAnimator.SetBool("IsWalking", true);
        Ctx.isWalking = true;
        AgentModifiers();
        RetreatBack();
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
    private void RetreatBack()
    {
        Vector3 targetPosition = Ctx.transform.position + (Ctx.transform.forward * -2f);
        Ctx.Agent.SetDestination(targetPosition);
        Ctx.transform.LookAt(new Vector3(Ctx.Player.position.x, Ctx.transform.position.y, Ctx.Player.position.z));
    }
    private void AgentModifiers()
    {
        Ctx.Agent.isStopped = false;
        Ctx.Agent.stoppingDistance = 0f;
        Ctx.Agent.speed = 1.5f;

    }
}
