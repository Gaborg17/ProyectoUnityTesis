using UnityEngine;

public class CameraController : MonoBehaviour
{
    private InputManager input;

    [SerializeField] private Transform cameraCenter;
    [SerializeField] private Transform target;
    [SerializeField] private float ySensibility;
    [SerializeField] private float xSensibility;
    [SerializeField] private float smoothness;


    [SerializeField] private float minVerticalAngle;
    [SerializeField] private float maxVerticalAngle;
    [SerializeField] private float camDistance;
    [SerializeField] private float aimDistance;
    [SerializeField] private float distanceLerp = 12f;

    [Header("Aim Offset (over-the-shoulder)")]
    [SerializeField] private Vector3 aimOffset = new Vector3(0.6f, 0.1f, 0f);
    [SerializeField] private float offsetLerp = 12f;

    [Header("Aim FOV")]
    [SerializeField] private Camera cam;
    [SerializeField] private float normalFov = 60f;
    [SerializeField] private float aimFov = 40f;
    [SerializeField] private float fovLerp = 12f;
    [Header("Wall Collision")]
    [SerializeField] private LayerMask collisionLayers;
    [SerializeField] private float collisionRadius = 0.3f;
    [SerializeField] private float safetyMargin = 0.15f;
    [SerializeField] private float minDistance = 0.5f;
    [SerializeField] private float collisionInSpeed = 25f;
    [SerializeField] private float collisionOutSpeed = 8f;
    private Vector2 smoothedCam;
    private Vector2 camPos;

    private float currentDistance;
    private float smoothedDistance;
    private Vector3 currentOffset;
    private float currentFov;
    public bool canAim;

    private void Start()
    {
        input = InputManager.Instance;
        GameManager.Instance.showCursor = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (cam == null) cam = GetComponent<Camera>();

        currentDistance = camDistance;
        smoothedDistance = camDistance + Mathf.Abs(cam.transform.localPosition.z); currentOffset = Vector3.zero;
        currentFov = normalFov;
    }

    private void Update()
    {
        if (GameManager.Instance.IsPaused == true) return;
        RotateCamera();
        UpdateAimState();
    }

    private void RotateCamera()
    {
        float x = input.MouseDeltaPirata().x * xSensibility;
        float y = input.MouseDeltaPirata().y * ySensibility;

        smoothedCam = Vector2.Lerp(smoothedCam, new Vector2(x,y), 1/ smoothness);
        camPos += smoothedCam;


        camPos.y = Mathf.Clamp(camPos.y,minVerticalAngle,maxVerticalAngle);

        cameraCenter.localRotation = Quaternion.Euler(-camPos.y, camPos.x, 0);
        float camLocalZ = Mathf.Abs(cam.transform.localPosition.z);
        float desiredDistance = currentDistance + camLocalZ;

        Vector3 dir = cameraCenter.localRotation * Vector3.back;

        float allowedDistance = desiredDistance;
        if (Physics.SphereCast(target.position,collisionRadius,dir,out RaycastHit hit,desiredDistance,collisionLayers,QueryTriggerInteraction.Ignore))
        {
            allowedDistance = Mathf.Clamp(hit.distance - safetyMargin, minDistance, desiredDistance);
        }

        float speed = allowedDistance < smoothedDistance ? collisionInSpeed : collisionOutSpeed;
        smoothedDistance = Mathf.Lerp(smoothedDistance, allowedDistance, Time.deltaTime * speed);

        float effectiveCamDistance = smoothedDistance - camLocalZ;
        Vector3 position = cameraCenter.localRotation * new Vector3(0f, 0f, -effectiveCamDistance) + target.position;
        cameraCenter.localPosition = position;
    }

    private void UpdateAimState()
    {
        if (!canAim) return;
        bool aiming = input.RightMouseClicked();

        float targetDistance = aiming ? aimDistance : camDistance;
        Vector3 targetOffset = aiming ? aimOffset : Vector3.zero;
        float targetFov = aiming ? aimFov : normalFov;

        float t = Time.deltaTime;

        currentDistance = Mathf.Lerp(currentDistance, targetDistance, t * distanceLerp);
        currentOffset = Vector3.Lerp(currentOffset, targetOffset, t * offsetLerp);
        currentFov = Mathf.Lerp(currentFov, targetFov, t * fovLerp);

        if (cam != null) cam.fieldOfView = currentFov;
    }
}
