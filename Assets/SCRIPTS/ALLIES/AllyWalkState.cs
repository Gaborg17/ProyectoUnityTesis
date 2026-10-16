using UnityEngine;
using UnityEngine.AI;

public class AllyWalkState : AllyBaseState
{
    public AllyWalkState(AllyStateMachine currentContext, AllyStateFactory allyStateFactory)
: base(currentContext, allyStateFactory) 
    {
        IsRootState = true;
    }
    public override void CheckSwitchState()
    {
        if (Ctx.InCombat)
        {
            SwitchState(Factory.Combat());
        }

        if (!Ctx.Agent.pathPending && Ctx.Agent.remainingDistance <= Ctx.Agent.stoppingDistance)
        {
            SwitchState(Factory.Idle());
        }
    }

    public override void EnterState()
    {
        Ctx.AllyAnimator.SetBool("IsWalking", true);
        Ctx.isWalking = true;
        
    }

    public override void ExitState()
    {
        //Ctx.transform.LookAt(new Vector3(Ctx.Player.transform.position.x, Ctx.transform.position.y, Ctx.Player.transform.position.z));

    }

    public override void InitializeSubState()
    {
    }

    public override void UpdateState()
    {
        SetDestination();
        CheckSwitchState();
    }

    public void SetDestination()
    {
        Vector3 backDistance = -Ctx.Player.forward;

        Vector3 lateralDisplacement = Ctx.Player.right * Ctx.playerSideStep;

        Vector3 destiny = Ctx.Player.position + (backDistance * Ctx.playerDistance) + lateralDisplacement;

        Ctx.Agent.SetDestination(destiny);
        Ctx.transform.LookAt(new Vector3(destiny.x, Ctx.transform.position.y,destiny.z));

        float distance = Vector3.Distance(Ctx.transform.position, Ctx.Player.transform.position);
        if (distance > 8)
        {
            Ctx.Agent.speed = 5;
        }
        else
        {
            Ctx.Agent.speed = 2;
        }

    }
}
