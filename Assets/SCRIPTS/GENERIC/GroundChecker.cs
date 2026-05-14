using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] private Transform grndChck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float detectionRange;

    public bool isGrounded;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(CheckRay().origin, CheckRay().direction * detectionRange);
    }

    public bool IsGrounded()
    {
        isGrounded = Physics.Raycast(CheckRay(), detectionRange, groundLayer);
        return isGrounded;
    }

    public Ray CheckRay()
    {
        return new Ray(grndChck.position, Vector3.down);
    }

}
