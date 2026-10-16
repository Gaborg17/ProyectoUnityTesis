using UnityEngine;

public class AllyIdleState : AllyBaseState
{
    public AllyIdleState(AllyStateMachine currentContext, AllyStateFactory allyStateFactory)
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

        float distance = Vector3.Distance(Ctx.transform.position, Ctx.Player.transform.position);
        if(distance > 3)
        {
            SwitchState(Factory.Walk());
        }
    }

    public override void EnterState()
    {
        Ctx.AllyAnimator.SetBool("IsWalking", false);
        Ctx.isWalking = false;
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
}
