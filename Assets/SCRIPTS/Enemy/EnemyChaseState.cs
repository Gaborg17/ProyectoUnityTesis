using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    public EnemyChaseState(EnemyStateMachine currentContext, EnemyStateFactory playerStateFactory)
: base(currentContext, playerStateFactory){}
    public override void CheckSwitchState()
    {
        if (!Ctx.Agent.pathPending && Ctx.Agent.remainingDistance <= Ctx.Agent.stoppingDistance)
        {
            SwitchState(Factory.Attack());
        }
    }

    public override void EnterState()   
    {
        AgentModifiers();
        Ctx.EnemyAnimator.SetBool("IsWalking", true);
        Ctx.Agent.updateRotation = false;
        
    }

    public override void ExitState()
    {
        Ctx.Agent.updateRotation = true;

    }

    public override void InitializeSubState()
    {
    }

    public override void UpdateState()
    {
        FollowPlayer();
        CheckSwitchState();
    }

    private void FollowPlayer()
    {
        Ctx.Agent.SetDestination(Ctx.AssignedTarget.position);
        Ctx.transform.LookAt(new Vector3(Ctx.AssignedTarget.position.x, Ctx.transform.position.y, Ctx.AssignedTarget.position.z));

    }

    private void AgentModifiers()
    {
        Ctx.Agent.isStopped = false;
        Ctx.Agent.stoppingDistance = .6f;
        Ctx.Agent.speed = 2f;

    }

}
