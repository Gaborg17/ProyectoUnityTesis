using UnityEngine;

public class AllyCombatState : AllyBaseState
{
    public AllyCombatState(AllyStateMachine currentContext, AllyStateFactory allyStateFactory)
: base(currentContext, allyStateFactory) 
    {
        IsRootState = true;
    }
    public override void CheckSwitchState()
    {
        if (Ctx.InCombat) return;

        float distance = Vector3.Distance(Ctx.transform.position, Ctx.Player.transform.position);
        if (distance > 3)
        {
            SwitchState(Factory.Walk());
        }
        else
        {
            SwitchState(Factory.Idle());
        }
    }

    public override void EnterState()
    {
        InitializeSubState();
    }

    public override void ExitState()
    {
    }

    public override void InitializeSubState()
    {
        SetSubState(Factory.Chase());
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }
}
