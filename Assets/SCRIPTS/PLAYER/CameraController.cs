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
    private Vector2 smoothedCam;
    private Vector2 camPos;

    private float currentDistance;
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
        currentOffset = Vector3.zero;
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
        Vector3 position = cameraCenter.localRotation * new Vector3(0.0f, 0.0f, -camDistance) + target.position;

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
