using UnityEngine;
using Cinemachine;

public class CameraPan : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform panPoint;
    [SerializeField] private Transform playerTarget;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    private Cinemachine3rdPersonFollow cinemachine3rdPersonFollow;

    [Space(10)]
    [Header("Pan Settings")]
    [SerializeField] private float panSpeed = 5f;
    [SerializeField] private float TopClamp = 70f;
    [SerializeField] private float BottomClamp = -40f;

    [Space(10)]
    [Header("Scroll Settings")]
    [SerializeField] private float scrollSpeed = 10f;
    [SerializeField] private float minFollowDistance = 2f;
    [SerializeField] private float maxFollowDistance = 20f;

    private float cinemachineTargetYaw;
    private float cinemachineTargetPitch;

    private void Awake()
    {
        cinemachine3rdPersonFollow = cinemachineVirtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
    }

    private void Start()
    {
        //Setting the initial values of the camera so that it doesn't start at (0,0), therefore causing a jump
        cinemachineTargetYaw = panPoint.eulerAngles.y;
        cinemachineTargetPitch = panPoint.eulerAngles.x;
    }

    private void LateUpdate()
    {
        if(Input.GetMouseButton(0) && Input.GetMouseButton(1) || Input.GetMouseButton(1))
        {
            PlayerAndCameraRotateLogic();
        }
        else if (Input.GetMouseButton(0))
            CameraRotateLogic();
        CameraZoom();
    }

    private void CalculateAndApplyMouseInput()
    {
        float mouseX = GetMouseInput("Mouse X");
        float mouseY = GetMouseInput("Mouse Y");

        cinemachineTargetYaw = UpdateRotation(cinemachineTargetYaw, mouseX, float.MinValue, float.MaxValue, false);
        cinemachineTargetPitch = UpdateRotation(cinemachineTargetPitch, mouseY, BottomClamp, TopClamp, true);
    }

    private void CameraRotateLogic()
    {
        CalculateAndApplyMouseInput();
        ApplyCameraRotation(cinemachineTargetPitch, cinemachineTargetYaw);
    }

    private void PlayerAndCameraRotateLogic()
    {
        CalculateAndApplyMouseInput();
        ApplyPlayerRotation(cinemachineTargetYaw);
        ApplyCameraRotation(cinemachineTargetPitch, cinemachineTargetYaw);
    }

    private void ApplyCameraRotation(float pitch, float yaw)
    {
        panPoint.rotation = Quaternion.Euler(pitch, yaw, panPoint.eulerAngles.z);
    }

    private void ApplyPlayerRotation(float yaw)
    {
        playerTarget.rotation = Quaternion.Euler(playerTarget.eulerAngles.x, yaw, playerTarget.eulerAngles.z);
    }

    private float UpdateRotation(float currentRotation, float input, float min, float max, bool isXAxis)
    {
        currentRotation += isXAxis ? -input : input;
        return Mathf.Clamp(currentRotation, min, max);
    }

    private void CameraZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        cinemachine3rdPersonFollow.CameraDistance = Mathf.Clamp(cinemachine3rdPersonFollow.CameraDistance - scrollInput * scrollSpeed, minFollowDistance, maxFollowDistance);
    }

    private float GetMouseInput(string axis)
    {
        return Input.GetAxis(axis) * panSpeed * Time.deltaTime;
    }
}
