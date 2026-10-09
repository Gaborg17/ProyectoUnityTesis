using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class PlayerInCombatState : PlayerBaseState
{


    public PlayerInCombatState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }
    public override void CheckSwitchState()
    {


    }

    public override void EnterState()
    {
        AttackCheck();
        //Ctx.PAnimator.SetTrigger(Ctx.IsAttackingHash);
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

    private void AttackCheck()
    {
        if(Ctx.EnemySelector.CurrentTarge()  == null)
        {
            Attack(null, 0);
            return;
        }
        else
        {
            Attack(Ctx.EnemySelector.target, TargetDistance(Ctx.EnemySelector.target));
        }
    }

    int animationCount = 0;
    string[] attacks;
    public void Attack(EnemyStateMachine target, float distance)
    {
        attacks = new string[] { "AirKick", "AirKick2", "AirPunch", "AirKick3" };
        if (target == null || target.gameObject.activeSelf == false)
        {
            AttackType("Hit", .2f, null, 0);
            return;
        }

        if (distance < 5 && distance > 0.7f)
        {
            Debug.Log("Distance");
            
            //animationCount = (int)Mathf.Repeat((float)animationCount + 1, (float)attacks.Length);
            //string attackString = isLastHit() ? attacks[Random.Range(0, attacks.Length)] : attacks[animationCount];
            AttackType("Hit", .2f, target, .25f);
        }
        else
        {
            //lockedTarget = null;
            AttackType("Hit", .2f, null, 0);
        }
    }

    private void AttackType(string attackTrigger, float cooldown, EnemyStateMachine target, float movementDuration)
    {
        Ctx.PAnimator.SetTrigger(attackTrigger);
        if (target == null)
            return;

        //target.StopMoving();
        Ctx.MoveToTarget(target, movementDuration);

        //IEnumerator AttackCoroutine(float duration)
        //{
        //    movementInput.acceleration = 0;
        //    isAttackingEnemy = true;
        //    movementInput.enabled = false;
        //    yield return new WaitForSeconds(duration);
        //    isAttackingEnemy = false;
        //    yield return new WaitForSeconds(.2f);
        //    movementInput.enabled = true;
        //    LerpCharacterAcceleration();
        //}
    }

    float TargetDistance(EnemyStateMachine target)
    {
        return Vector3.Distance(Ctx.transform.position, target.transform.position);
    }


}
