using UnityEngine;

public class EnemySelection : MonoBehaviour
{
    [SerializeField] Vector3 inputDirection;
    public LayerMask layerMask;

    public Camera cam;
    public GameObject target;
    void Start()
    {
        
    }

    void Update()
    {
        SelectEnemy();
    }

    private void SelectEnemy()
    {
        
        var forward = cam.transform.forward;
        var right = cam.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        inputDirection = forward * InputManager.Instance.MoveDirection().y + right * InputManager.Instance.MoveDirection().x;
        inputDirection = inputDirection.normalized;

        RaycastHit info;

        if (Physics.SphereCast(transform.position, 3f, inputDirection, out info, 5, layerMask))
        {
            Debug.Log(info.collider.gameObject.name);
            target = info.collider.gameObject;
            //if (info.collider.transform.GetComponent<EnemyScript>().IsAttackable())
            //    currentTarget = info.collider.transform.GetComponent<EnemyScript>();
        }
    }

    public GameObject CurrentTarge()
    {
        return target;
    }

    public void SetCurrentTarget(GameObject obj)
    {
        target = obj;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, inputDirection);
        Gizmos.DrawWireSphere(transform.position, 1);
        if (target != null)
            Gizmos.DrawSphere(target.transform.position, .5f);
    }
}
