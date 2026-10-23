using UnityEngine;

public class PlayerRangedState : PlayerBaseState
{
    public PlayerRangedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }
    public override void CheckSwitchState()
    {
        if (!InputManager.Instance.RightMouseClicked())
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

    public override void EnterState()
    {
        Ctx.PAnimator.SetBool(Ctx.IsWalkingHash, false);
        Ctx.PAnimator.SetBool(Ctx.IsRunningHash, false);
        Ctx.isWalking = false;
        Ctx.isAiming = true;
        Ctx.crosshair.SetActive(true);
        Debug.Log("Aiming");
        
    }

    public override void ExitState()
    {
        Ctx.isAiming = false;
        Ctx.crosshair.SetActive(false);
        Ctx.canShoot = true;

    }

    public override void InitializeSubState()
    {
    }

    public override void UpdateState()
    {
        CheckSwitchState();
        if (Ctx.InputManager.AttackIsPressed() && Ctx.canShoot)
        {
            Shoot();
        }
    }



    public void Shoot()
    {
        Ctx.canShoot = false;

        Ray ray = shootRay();
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10f, Ctx.damageMasks))
        {
            var damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable != null)
                damageable.OnTakeDamage(Ctx.Damage * 10);

            Debug.DrawLine(ray.origin, hit.point, Color.red, 1f);
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * 10f, Color.green, 1f);
        }
    }

    public Ray shootRay()
    {
        return new Ray(
        Ctx.cameraController.transform.position,
        Ctx.cameraController.transform.forward
    );
    }
}
