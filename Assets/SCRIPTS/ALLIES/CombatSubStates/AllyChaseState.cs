using UnityEngine;

public class AllyChaseState : AllyBaseState
{
    public AllyChaseState(AllyStateMachine currentContext, AllyStateFactory allyStateFactory)
: base(currentContext, allyStateFactory) { }
    public override void CheckSwitchState()
    {

        if (!Ctx.Agent.pathPending && Ctx.Agent.remainingDistance <= Ctx.Agent.stoppingDistance)
        {
            SwitchState(Factory.Attack());
        }
    }

    public override void EnterState()
    {
        Ctx.AllyAnimator.SetBool("IsWalking", true);
        Ctx.isWalking = true;
        AgentModifiers();
    }

    public override void ExitState()
    {
    }

    public override void InitializeSubState()
    {
    }

    public override void UpdateState()
    {
        GoToEnemy();
        CheckSwitchState();
    }

    private void GoToEnemy()
    {
        if (Ctx.AssignedEnemy == null)
        {
            Ctx.InCombat = false;
            return;
        }
        Ctx.Agent.SetDestination(Ctx.AssignedEnemy.position);
        Ctx.transform.LookAt(new Vector3(Ctx.AssignedEnemy.position.x, Ctx.transform.position.y, Ctx.AssignedEnemy.position.z));

    }
    private void AgentModifiers()
    {
        Ctx.Agent.isStopped = false;
        Ctx.Agent.stoppingDistance = .6f;
        Ctx.Agent.speed = 2f;

    }
}
