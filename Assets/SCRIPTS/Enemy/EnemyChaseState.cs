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
        Debug.Log("Chase");
        AgentModifiers();
        Ctx.EnemyAnimator.SetBool("IsWalking", true);
    }

    public override void ExitState()
    {

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
        Ctx.Agent.SetDestination(Ctx.Player.position);
        

    }

    private void AgentModifiers()
    {
        Debug.Log("Modify");

        Ctx.Agent.isStopped = false;
        Ctx.Agent.stoppingDistance = .6f;
    }

}
