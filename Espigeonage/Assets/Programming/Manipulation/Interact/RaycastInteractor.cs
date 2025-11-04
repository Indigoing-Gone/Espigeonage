using UnityEngine;

public class RaycastInteractor : Interactor
{
    [Header("Components")]
    private Transform origin;
    public Transform Origin { get => origin; set { if (!origin) origin = value; } }

    [Header("Parameters")]
    [SerializeField] private float interactDistance;

    protected override void HandleInteract()
    {
        bool _hit = Physics.Raycast(origin.position, origin.forward, out RaycastHit _hitInfo,
            interactDistance, interactLayer, QueryTriggerInteraction.Collide);

        if (!_hit) return;

        _hitInfo.transform.TryGetComponent<IInteractable>(out IInteractable _interactable);
        _interactable?.Interact(this);
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        Debug.DrawLine(origin.position, origin.position + (origin.forward * interactDistance), Color.red);
    }
}
