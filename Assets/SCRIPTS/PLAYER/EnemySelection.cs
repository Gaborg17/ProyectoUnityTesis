using UnityEngine;

public class EnemySelection : MonoBehaviour
{
    [SerializeField] Vector3 inputDirection;
    public LayerMask layerMask;

    public Camera cam;
    public EnemyStateMachine target;
    void Start()
    {
        
    }

    void Update()
    {
        SelectEnemy();
    }

    private void SelectEnemy()
    {
        
        //var forward = cam.transform.forward;
        //var right = cam.transform.right;

        //forward.y = 0f;
        //right.y = 0f;

        //forward.Normalize();
        //right.Normalize();

        //inputDirection = forward * InputManager.Instance.MoveDirection().y + right * InputManager.Instance.MoveDirection().x;
        //inputDirection = inputDirection.normalized;

        RaycastHit info;

        if (Physics.SphereCast(transform.position, 3f, transform.forward, out info, 5, layerMask))
        {
            target = info.collider.transform.GetComponent<EnemyStateMachine>();
            //if (info.collider.transform.GetComponent<EnemyScript>().IsAttackable())
            //    currentTarget = info.collider.transform.GetComponent<EnemyScript>();
        }
    }

    public EnemyStateMachine CurrentTarge()
    {
        return target;
    }

    public void SetCurrentTarget(EnemyStateMachine enemyStateMachine)
    {
        target = enemyStateMachine;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawRay(transform.position, inputDirection);
        Gizmos.DrawWireSphere(transform.position, 1);
        if (target != null)
            Gizmos.DrawWireSphere(target.transform.position, .5f);
    }
}
