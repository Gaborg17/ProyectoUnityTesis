using UnityEngine;
using UnityEngine.UIElements;

public class EnemyRetreatState : EnemyBaseState
{
    public EnemyRetreatState(EnemyStateMachine currentContext, EnemyStateFactory playerStateFactory)
: base(currentContext, playerStateFactory) { }

    public override void CheckSwitchState()
    {
        if (!Ctx.Agent.pathPending && Ctx.Agent.remainingDistance <= Ctx.Agent.stoppingDistance)
        {
            SwitchState(Factory.Reposition());
        }
    }

    public override void EnterState()
    {
        Ctx.Agent.updateRotation = false;
        AgentModifiers();
        Ctx.EnemyAnimator.SetBool("IsWalking", true);
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
        Ctx.transform.LookAt(Ctx.Player.position);
    }
    private void AgentModifiers()
    {
        Ctx.Agent.isStopped = false;
        Ctx.Agent.stoppingDistance = 0f;
    }
}

