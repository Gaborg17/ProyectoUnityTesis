using UnityEngine;

public class AllyAttackState : AllyBaseState
{
    private float attackDuration = 1f;
    private float timer;

    private bool attacked;
    public AllyAttackState(AllyStateMachine currentContext, AllyStateFactory allyStateFactory)
: base(currentContext, allyStateFactory) { }
    public override void CheckSwitchState()
    {
        SwitchState(Factory.Retreat());
    }

    public override void EnterState()
    {
        Ctx.AllyAnimator.SetBool("IsWalking", false);
        Ctx.isWalking = false;
        Ctx.Agent.isStopped = true;
        timer = 0;
    }

    public override void ExitState()
    {
        timer = 0;
        attacked = false;
    }

    public override void InitializeSubState()
    {
    }

    public override void UpdateState()
    {
        timer += Time.deltaTime;

        if (timer >= 0.45f && attacked == false)
        {
            Ctx.AllyAnimator.SetTrigger("Hit");
            Ctx.DamageZone.enabled = true;
            attacked = true;

        }

        if (timer >= attackDuration)
        {
            Ctx.DamageZone.enabled = false;
            CheckSwitchState();

        }
    }
}
