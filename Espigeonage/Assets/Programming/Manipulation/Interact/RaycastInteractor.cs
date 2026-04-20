using UnityEngine;

public class RaycastInteractor : Interactor
{
    [Header("Parameters")]
    [SerializeField] private float interactDistance;
    private Ray ray;

    public override void FindInteractables()
    {
        //Raycast to find interactables
        bool _hit = Physics.Raycast(ray, out RaycastHit _hitInfo,
            interactDistance, interactLayer, QueryTriggerInteraction.Collide);

        IInteractable _targetInteractable = null;

        //If something is hit and its not  UI, try to get the Interactable
        if (_hit && !_hitInfo.collider.TryGetComponent<RectTransformBoxCollider>(out _))
        {
            _hitInfo.collider.TryGetComponent<IInteractable>(out IInteractable _direct);
            _hitInfo.transform.TryGetComponent<IInteractable>(out IInteractable _parent);

            //Target intercactable can be the directly hit object or its parent
            _targetInteractable = _direct ?? _parent;
        }

        //Update the found interactable
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
