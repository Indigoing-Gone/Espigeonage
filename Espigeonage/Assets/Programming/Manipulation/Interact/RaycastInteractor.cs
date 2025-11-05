using UnityEngine;

public class RaycastInteractor : Interactor
{
    [Header("Parameters")]
    [SerializeField] private float interactDistance;
    private Ray ray;

    private Vector3 origin;
    private Vector3 direction;

    protected override void HandleInteract()
    {
        //Ray _ray = new(origin, direction);
        bool _hit = Physics.Raycast(ray, out RaycastHit _hitInfo,
            interactDistance, interactLayer, QueryTriggerInteraction.Collide);

        if (!_hit) return;

        _hitInfo.transform.TryGetComponent<IInteractable>(out IInteractable _interactable);
        _interactable?.Interact(this);
    }

    public void UpdateRay(Vector3 _origin, Vector3 _direction)
    {
        origin = _origin;
        direction = _direction;

        ray = new(origin, direction);
    }

    public void UpdateRay(Ray _ray)
    {
        ray = _ray;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        Debug.DrawLine(ray.origin, ray.origin + (ray.direction * interactDistance), Color.red);
    }
}
