using System;
using UnityEngine;

public abstract class Interactor : MonoBehaviour
{
    public event Action<IInteractable, InteractionData> TargetInteractableUpdated;

    [Header("Components")]
    protected IInteractable targetInteractable;
    public IInteractable TargetInteractable => targetInteractable;

    [Header("Parameters")]
    [SerializeField] protected LayerMask interactLayer;
    [SerializeField] protected ActionState currentActionState;

    protected bool canInteract;

    protected virtual void Awake()
    {
        canInteract = true;
    }

    protected virtual void AttemptInteract() => targetInteractable?.Interact(this, currentActionState);
    protected virtual void UpdateTargetInteractable(IInteractable _newTarget)
    {
        if (targetInteractable == _newTarget) return;

        InteractionData _targetInteraction = default;
        MonoBehaviour _targetInteractableObject = _newTarget as MonoBehaviour;

        if (_targetInteractableObject != null &&
            _newTarget.TryFindInteraction(currentActionState, out _targetInteraction) &&
            !_targetInteraction.behaviour.Verify(_targetInteractableObject, this))
                _targetInteraction = default;

        if (_targetInteraction.behaviour == null) _newTarget = null;
        if (targetInteractable == _newTarget) return;

        targetInteractable = _newTarget;
        TargetInteractableUpdated?.Invoke(targetInteractable, _targetInteraction);
    }
    public abstract void FindInteractables();

    public void TriggerInteraction() { if (canInteract) AttemptInteract(); }
    public void SetActionState(ActionState _newActionState)
    {
        currentActionState = _newActionState;

        if (targetInteractable == null)
        {
            FindInteractables();
            return;
        }

        targetInteractable.TryFindInteraction(currentActionState, out InteractionData _targetInteraction);
        TargetInteractableUpdated?.Invoke(targetInteractable, _targetInteraction);
    }
}
