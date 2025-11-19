using System;
using UnityEngine;

public abstract class Interactor : MonoBehaviour
{
    public event Action<IInteractable> TargetInteractableUpdated;

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
        targetInteractable = _newTarget;
        TargetInteractableUpdated?.Invoke(targetInteractable);
    }
    public abstract void FindInteractables();

    public void TriggerInteraction() { if (canInteract) AttemptInteract(); }
    public void SetActionState(ActionState _newActionState) => currentActionState = _newActionState;
}
