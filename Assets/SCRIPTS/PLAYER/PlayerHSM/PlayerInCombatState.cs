using UnityEngine;

public class PlayerInCombatState : PlayerBaseState
{

    
    public PlayerInCombatState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }
    public override void CheckSwitchState()
    {


    }

    public override void EnterState()
    {
        Ctx.PAnimator.SetTrigger(Ctx.IsAttackingHash);
        Ctx.temporalDamageCollider.SetActive(true);
        
    }

    public override void ExitState()
    {
        Ctx.temporalDamageCollider.SetActive(false);

    }

    public override void InitializeSubState()
    {
    }

    public override void UpdateState()
    {
        ChecarAnim();
        CheckSwitchState();
    }

    private void ChecarAnim()
    {
        AnimatorStateInfo stateInfo = Ctx.PAnimator.GetCurrentAnimatorStateInfo(0);
        if (!stateInfo.IsName("Hit")) return;
        if (stateInfo.normalizedTime >= 0.95f && !stateInfo.loop)
        {
            SwitchState(Factory.Walk());
        }
    }
}
