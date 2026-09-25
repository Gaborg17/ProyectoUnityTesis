using Unity.VisualScripting;
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

    private Vector2 smoothedCam;
    private Vector2 camPos;



    private void Start()
    {
        input = InputManager.Instance;
        GameManager.Instance.showCursor = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (GameManager.Instance.IsPaused == true) return;
        RotateCamera();
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


}
