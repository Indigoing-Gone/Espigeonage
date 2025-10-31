using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] CinemachineInputAxisController cameraInputController;
    [SerializeField] Transform orientation;

    [Header("Camera Settings")]
    [SerializeField] Vector2 pitchMinMax;
    [SerializeField] Vector2 sensitivity;

    [Header("Calculations")]
    private Vector2 lookDirection;
    float pitch;
    float yaw;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        pitch = 0;
        yaw = 0;
    }

    private void LateUpdate()
    {
        float mouseX = lookDirection.x * sensitivity.x * Time.fixedDeltaTime;
        float mouseY = lookDirection.y * sensitivity.y * Time.fixedDeltaTime;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch - mouseY, pitchMinMax.x, pitchMinMax.y);

        orientation.rotation = Quaternion.Euler(pitch, yaw, 0);
    }

    public void ProcessInput(Vector2 _direction)
    {
        lookDirection = _direction;
    }
}
