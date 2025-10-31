using UnityEngine;

public class RaycastInteractor : Interactor
{
    [Header("Components")]
    private Transform orientation;
    public Transform Orientation { get => orientation; set { if (!orientation) orientation = value; } }

    [Header("Parameters")]
    [SerializeField] private float interactDistance;

    protected override void HandleInteract()
    {
        bool _hit = Physics.Raycast(orientation.position, orientation.forward, out RaycastHit _hitInfo,
            interactDistance, interactLayer, QueryTriggerInteraction.Ignore);

        if (!_hit) return;

        _hitInfo.transform.TryGetComponent<IInteractable>(out IInteractable _interactable);
        _interactable?.Interact(this);
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        Debug.DrawLine(orientation.position, orientation.position + (orientation.forward * interactDistance), Color.red);
    }
}
