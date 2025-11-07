using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FirstPersonMovement : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody rb;
    private Transform orientation;
    public Transform Orientation { get => orientation; set { if (!orientation) orientation = value; } }

    [Header("Movement")]
    [SerializeField] private float maxSpeed = 300;
    [SerializeField] private float acceleration = 80;
    [SerializeField] private float deceleration = 80;

    private Vector2 moveDirection;
    private bool pressingKey;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        float _maxSpeedChange = (pressingKey ? acceleration : deceleration) * Time.fixedDeltaTime;

        Vector3 _moveOrientation = Quaternion.AngleAxis(orientation.rotation.eulerAngles.y, Vector3.up) * new Vector3(moveDirection.x, 0, moveDirection.y);
        Vector3 _moveVector = transform.TransformDirection(_moveOrientation) * Time.fixedDeltaTime;

        Vector3 _currentXZVelocity = Vector3.Scale(rb.linearVelocity, new(1, 0, 1));
        Vector3 _targetXZVelocity = _moveVector * maxSpeed;

        //Set velocity method
        //Vector3 _newXZVelocity = Vector3.MoveTowards(_currentXZVelocity, _targetXZVelocity, _maxSpeedChange);
        //rb.linearVelocity = rb.linearVelocity - _currentXZVelocity + _newXZVelocity;

        //Apply force method
        Vector3 _velocityChange = (_targetXZVelocity - _currentXZVelocity);
        float _changeMagnitude = Mathf.Clamp(_velocityChange.magnitude, -_maxSpeedChange, _maxSpeedChange);
        rb.AddForce(_changeMagnitude * _velocityChange.normalized, ForceMode.VelocityChange);
    }

    public void SetMoveDirection(Vector2 _direction)
    {
        moveDirection = _direction;
        pressingKey = _direction != Vector2.zero;
    }
}
