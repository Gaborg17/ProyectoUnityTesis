using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    private float attackDuration = 5f;
    private float timer;

    public EnemyAttackState(EnemyStateMachine currentContext, EnemyStateFactory playerStateFactory)
: base(currentContext, playerStateFactory) { }
    public override void CheckSwitchState()
    {
        SwitchState(Factory.Chase());
    }

    public override void EnterState()
    {
        timer = 0f;

        Ctx.Agent.isStopped = true;
        //AttackAnimation
        Ctx.EnemyAnimator.SetTrigger("Hit");
        Ctx.DamageZone.enabled = true;

        Ctx.EnemyAnimator.SetBool("IsWalking", false);
    }

    public override void ExitState()
    {

    }

    public override void InitializeSubState()
    {
    }

    public override void UpdateState()
    {
        timer += Time.deltaTime;

        if(timer >= 0.3f)
        {
            Ctx.DamageZone.enabled = false;
        }

        if (timer >= attackDuration)
        {
            CheckSwitchState();

        }
    }




}
