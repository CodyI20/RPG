using Cinemachine;
using UnityEngine;

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
    [SerializeField, Range(0, 50)] private float cameraScrollSmoothness = 5f;

    [Space(10)]
    [Header("Camera Settings")]
    [SerializeField] private bool repositionCameraToBackWhileWalking = true;
    [SerializeField, Range(0, 20)] private float smoothRotationFactor = 0.2f;

    private float cinemachineTargetYaw;
    private float cinemachineTargetPitch;

    private bool isPlayerDead = false;

    private void OnEnable()
    {
        PlayerMovement.OnPlayerAttemptingMove += LerpCameraBack;
        PlayerStats.Instance.OnPlayerDeath += () => isPlayerDead = true;
    }

    private void OnDisable()
    {
        PlayerMovement.OnPlayerAttemptingMove -= LerpCameraBack;
        PlayerStats.Instance.OnPlayerDeath -= () => isPlayerDead = true;
    }

    private void Awake()
    {
        cinemachine3rdPersonFollow = cinemachineVirtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
    }

    private void Start()
    {
        //Setting the initial values of the camera so that it doesn't start at (0,0), therefore causing a jump
        cinemachineTargetYaw = panPoint.eulerAngles.y;
        cinemachineTargetPitch = panPoint.eulerAngles.x;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButton(0) && Input.GetMouseButton(1) || Input.GetMouseButton(1))
        {
            HideCursor();
            if (!isPlayerDead)
            {
                PlayerAndCameraRotateLogic();
            }
            else
            {
                CameraRotateLogic();
            }
        }
        else if (Input.GetMouseButton(0))
        {
            HideCursor();
            CameraRotateLogic();
        }
        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            ShowCursor();
        }
        CameraZoom();
    }

    private void HideCursor()
    {
        Cursor.visible = false;
    }

    private void ShowCursor()
    {
        Cursor.visible = true;
    }

    private void LerpCameraBack(GameObject gameObject)
    {
        if (gameObject == playerTarget.gameObject && repositionCameraToBackWhileWalking && Cursor.visible == true) //If it's not currently overriden by the player's actions
            SmoothCameraRotation();
    }

    private void CalculateAndApplyMouseInput()
    {
        float mouseX = GetMouseInput("Mouse X");
        float mouseY = GetMouseInput("Mouse Y");

        mouseY = Mathf.Clamp(mouseY, BottomClamp, TopClamp);

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

    private void SmoothCameraRotation()
    {
        panPoint.rotation = Quaternion.Euler(Mathf.LerpAngle(panPoint.eulerAngles.x, playerTarget.eulerAngles.x, smoothRotationFactor * Time.deltaTime), Mathf.LerpAngle(panPoint.eulerAngles.y, playerTarget.eulerAngles.y, smoothRotationFactor * Time.deltaTime), panPoint.eulerAngles.z);
        cinemachineTargetYaw = panPoint.eulerAngles.y;
        cinemachineTargetPitch = panPoint.eulerAngles.x;
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
        return currentRotation;
    }

    private void CameraZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        float intermediateValue = Mathf.Lerp(cinemachine3rdPersonFollow.CameraDistance, cinemachine3rdPersonFollow.CameraDistance - scrollInput * scrollSpeed, Time.deltaTime * cameraScrollSmoothness);
        cinemachine3rdPersonFollow.CameraDistance = Mathf.Clamp(intermediateValue, minFollowDistance, maxFollowDistance);
    }

    private float GetMouseInput(string axis)
    {
        return Input.GetAxis(axis) * panSpeed * Time.deltaTime;
    }
}
