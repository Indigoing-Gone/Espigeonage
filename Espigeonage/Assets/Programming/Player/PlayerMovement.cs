using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody rb;
    [SerializeField] private Transform orientation;

    [Header("Movement")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;

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
        _velocityChange.x = Mathf.Clamp(_velocityChange.x, -_maxSpeedChange, _maxSpeedChange);
        _velocityChange.z = Mathf.Clamp(_velocityChange.z, -_maxSpeedChange, _maxSpeedChange);
        rb.AddForce(_velocityChange, ForceMode.VelocityChange);
    }

    public void ProcessInput(Vector2 _direction)
    {
        moveDirection = _direction;
        pressingKey = _direction != Vector2.zero;
    }
}
