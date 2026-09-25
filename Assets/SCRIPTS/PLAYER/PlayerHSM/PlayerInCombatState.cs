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
        Ctx.isWalking = false;

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
        if (!stateInfo.IsName("Armature|Punch_Cross")) return;
        if (stateInfo.normalizedTime >= 0.95f && !stateInfo.loop)
        {

            if (Ctx.InputManager.MoveDirection().magnitude < 0.1f)
            {
                SwitchState(Factory.Idle());
            }
            else
            {
                if (Ctx.InputManager.isRunning() == true)
                {
                    SwitchState(Factory.Run());
                }
                else
                {
                    SwitchState(Factory.Walk());
                }
            }

        }
    }
}
