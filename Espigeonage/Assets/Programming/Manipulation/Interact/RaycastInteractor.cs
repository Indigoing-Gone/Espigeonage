using UnityEngine;

public class RaycastInteractor : Interactor
{
    [Header("Parameters")]
    [SerializeField] private float interactDistance;
    private Ray ray;

    public override void FindInteractables()
    {
        bool _hit = Physics.Raycast(ray, out RaycastHit _hitInfo,
            interactDistance, interactLayer, QueryTriggerInteraction.Collide);

        IInteractable _targetInteractable = null;
        if (_hit && !_hitInfo.collider.TryGetComponent<RectTransformBoxCollider>(out _) && _hitInfo.transform.TryGetComponent<IInteractable>(out IInteractable _interactable))
            _targetInteractable = _interactable;

        UpdateTargetInteractable(_targetInteractable);
    }

    public void UpdateRay(Vector3 _origin, Vector3 _direction) => ray = new(_origin, _direction);
    public void UpdateRay(Vector2 _screenPosition) => ray = Camera.main.ScreenPointToRay(_screenPosition);
    public void UpdateRay(Ray _ray) => ray = _ray;

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        Debug.DrawLine(ray.origin, ray.origin + (ray.direction * interactDistance), Color.red);
    }
}
