using UnityEngine;
public class EnemyCombatState : EnemyBaseState
{
    public EnemyCombatState(EnemyStateMachine currentContext, EnemyStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory)
    {
        IsRootState = true;

    }
    public override void CheckSwitchState()
    {
        if (Ctx.InCombat == false)
        {
            SwitchState(Factory.Grounded());
        }
    }

    public override void EnterState()
    {
        InitializeSubState();
        Ctx.Agent.isStopped = false;
    }

    public override void ExitState()
    {

    }

    public override void InitializeSubState()
    {
        if(Ctx.InCombat == true)
        {
            Debug.Log("Incializando Chase");
            SetSubState(Factory.Chase());
        }
    


    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }
}
