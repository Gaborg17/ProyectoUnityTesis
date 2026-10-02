using UnityEngine;

public class EnemyRepositionState : EnemyBaseState
{
    public EnemyRepositionState(EnemyStateMachine currentContext, EnemyStateFactory playerStateFactory)
: base(currentContext, playerStateFactory) { }

    public override void CheckSwitchState()
    {
        if (!Ctx.Agent.pathPending && Ctx.Agent.remainingDistance <= Ctx.Agent.stoppingDistance)
        {
            SwitchState(Factory.Chase());
        }
    }

    public override void EnterState()
    {
        Ctx.Agent.updateRotation = false;
        AgentModifiers();
        Ctx.EnemyAnimator.SetBool("IsWalking", true);
        //RepositionAroundPlayer();
        StartReposition();
    }

    public override void ExitState()
    {
        Ctx.Agent.updateRotation = true;
    }

    public override void InitializeSubState()
    {
    }

    public override void UpdateState()
    {
        RepositionAroundPlayer();
        CheckSwitchState();
    }
    private float currentAngle;
    private float targetAngle;
    private bool isRepositioning = false;
    public float orbitRadius = 2f;
    public float orbitSpeed = 45f;
    public void StartReposition()
    {
        Vector3 directionToAgent = Ctx.transform.position - Ctx.Player.position;
        currentAngle = Mathf.Atan2(directionToAgent.z, directionToAgent.x) * Mathf.Rad2Deg;

        float randomOffset = Random.Range(60f, 120f);

        float direction = Random.value > 0.5f ? 1f : -1f;

        targetAngle = currentAngle + (randomOffset * direction);
        isRepositioning = true;
    }
    private void RepositionAroundPlayer()
    {
        if (!isRepositioning) return;

        float angleStep = orbitSpeed * Time.deltaTime;

        currentAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, angleStep);

        float x = Mathf.Cos(currentAngle * Mathf.Deg2Rad) * orbitRadius;
        float z = Mathf.Sin(currentAngle * Mathf.Deg2Rad) * orbitRadius;

        Vector3 targetPosition = Ctx.Player.position + new Vector3(x, 0, z);
        Ctx.Agent.SetDestination(targetPosition);
        Ctx.transform.LookAt(Ctx.Player.position);

        if (Mathf.Approximately(currentAngle, targetAngle))
        {
            isRepositioning = false;
            Ctx.Agent.ResetPath();
        }
    }
    private void AgentModifiers()
    {
        Ctx.Agent.isStopped = false;
        Ctx.Agent.stoppingDistance = 0f;
    }

    private int RandomPosition()
    {
        int rand = Random.Range(-1, 1);
        if(rand == 0)
        {
            rand = RandomPosition();
        }
        return rand;
    }
}
