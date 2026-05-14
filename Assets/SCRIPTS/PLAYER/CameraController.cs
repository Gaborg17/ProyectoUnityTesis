using UnityEngine;

public class CameraController : MonoBehaviour
{
    private InputManager input;

    [SerializeField] private Transform cameraCenter;
    [SerializeField] private float sensibility;
    [SerializeField] private float smoothness;


    [SerializeField] private float minVerticalAngle;
    [SerializeField] private float maxVerticalAngle;

    private Vector2 mouseScaledPos;
    private Vector2 smoothedCam;
    private Vector2 camPos;

    private void Start()
    {
        input = InputManager.Instance;
    }

    private void Update()
    {
        RotateCamera();
    }

    private void RotateCamera()
    {
        mouseScaledPos = Vector2.Scale(input.MouseDeltaPirata(), Vector2.one *  sensibility);
        smoothedCam = Vector2.Lerp(smoothedCam, mouseScaledPos, 1 / smoothness);
        camPos += smoothedCam;

        camPos.y = Mathf.Clamp(camPos.y,minVerticalAngle,maxVerticalAngle);

        cameraCenter.localRotation = Quaternion.AngleAxis(-camPos.y, Vector3.right);
        transform.localRotation = Quaternion.AngleAxis(camPos.x, Vector3.up);
    }


}
