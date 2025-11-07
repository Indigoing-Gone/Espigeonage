using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [Header("Components")]
    private Transform orientation;
    public Transform Orientation { get => orientation; set { if (!orientation) orientation = value; } }

    [Header("Camera Settings")]
    [SerializeField] private Vector2 pitchMinMax = new(-90, 90);
    [SerializeField] private Vector2 sensitivity = new(4, 4);

    [Header("Calculations")]
    private Vector2 lookDirection;
    float pitch;
    float yaw;

    private void Awake()
    {
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

    public void SetLookDirection(Vector2 _direction)
    {
        lookDirection = _direction;
    }
}
