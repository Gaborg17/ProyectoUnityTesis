using UnityEngine;

public class NpcIdleState : NpcBaseState
{
    public NpcIdleState(NpcStateMachine currentContext, NpcStateFactory npcStateFactory)
: base(currentContext, npcStateFactory)
    {
        IsRootState = true;

    }
    public override void CheckSwitchState()
    {
        if (Ctx.Agent.isStopped == false)
        {
            SwitchState(Factory.Walk());

        }

        if (Ctx.Escaping == true)
        {
            SwitchState(Factory.IsEscaping());
        }
    }

    public override void EnterState()
    {
        Debug.Log("Idle");
        Ctx.isWalking = false;
        Ctx.Escaping = false;
        
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
