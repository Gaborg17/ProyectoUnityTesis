using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    private float attackDuration = 1f;
    private float timer;

    private bool attacked;
    public EnemyAttackState(EnemyStateMachine currentContext, EnemyStateFactory playerStateFactory)
: base(currentContext, playerStateFactory) { }
    public override void CheckSwitchState()
    {
        SwitchState(Factory.Retreat());
    }

    public override void EnterState()
    {
        timer = 0f;

        Ctx.Agent.isStopped = true;
        //ChargeAnimation
        
        

        Ctx.EnemyAnimator.SetBool("IsWalking", false);
    }

    public override void ExitState()
    {
        attacked = false;

    }

    public override void InitializeSubState()
    {
    }

    public override void UpdateState()
    {
        timer += Time.deltaTime;

        if(timer >= 0.45f && attacked == false)
        {
            Ctx.EnemyAnimator.SetTrigger("Hit");
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
