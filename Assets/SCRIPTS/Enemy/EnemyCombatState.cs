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


    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }
}
