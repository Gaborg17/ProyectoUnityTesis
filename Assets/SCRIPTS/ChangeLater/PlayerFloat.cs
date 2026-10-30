using UnityEngine;

public class PlayerFloat : MonoBehaviour
{
    [Header("Hover")]
    [SerializeField] private float hoverOffsetY = 1f;
    [SerializeField] private float stiffness = 25f;
    [SerializeField] private float damping = 2.5f;

    [Header("Límites")]
    [SerializeField] private float maxAccel = 60f;
    [SerializeField] private float maxError = 1.5f;
    [SerializeField] private float maxVelocity = 15f;

    [Header("Bobbing")]
    [SerializeField] private bool addBob = true;
    [SerializeField] private float bobAmplitude = 0.2f;
    [SerializeField] private float bobFrequency = 0.8f;

    private Rigidbody rb;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        other.TryGetComponent(out rb);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        rb = null;
    }

    private void FixedUpdate()
    {
        if (rb == null) return;

        float targetY = transform.position.y + hoverOffsetY;

        if (addBob)
            targetY += Mathf.Sin(Time.time * bobFrequency * Mathf.PI * 2f) * bobAmplitude;

        float error = Mathf.Clamp(targetY - rb.position.y, -maxError, maxError);
        float vy = Mathf.Clamp(rb.linearVelocity.y, -maxVelocity, maxVelocity);

        float accel = error * stiffness - vy * damping + Physics.gravity.magnitude;
        accel = Mathf.Clamp(accel, -maxAccel, maxAccel);

        rb.AddForce(Vector3.up * accel, ForceMode.Acceleration);
    }
}

