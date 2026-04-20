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

    //If we have a target interactable, interact with it -- on input
    protected virtual void AttemptInteract() => targetInteractable?.Interact(this, currentActionState);
    protected virtual void UpdateTargetInteractable(IInteractable _newTarget)
    {
        //If the new target is the same as the current, do nothing
        if (targetInteractable == _newTarget) return;

        InteractionData _targetInteraction = default;
        MonoBehaviour _targetInteractableObject = _newTarget as MonoBehaviour;

        //Do null check -- Try to find an interaction on the object that works in the current state -- verify interaction is allowed (mostly for grab related interactions)
        //If we cant verify the interaction, treat it as if we found no interaction
        if (_targetInteractableObject != null &&
            _newTarget.TryFindInteraction(currentActionState, out _targetInteraction) &&
            !_targetInteraction.behaviour.Verify(_targetInteractableObject, this))
                _targetInteraction = default;

        //If we forgot to add a behaviour to the found interaction or we found no interaction, null the target
        if (_targetInteraction.behaviour == null) _newTarget = null;
        //Double check against the current target cause we changed the _newTarget
        if (targetInteractable == _newTarget) return;

        targetInteractable = _newTarget;

        //Update tooltip and cursor UI
        TargetInteractableUpdated?.Invoke(targetInteractable, _targetInteraction);
    }
    public abstract void FindInteractables();

    //If interacting is allowed, attempt to interact -- on input -- added in case AttemptInteract needs extra logic in derived classes
    public void TriggerInteraction() { if (canInteract) AttemptInteract(); }
    public void SetActionState(ActionState _newActionState)
    {
        currentActionState = _newActionState;

        //Check for updating tooltip and cursor UI -- needed as the player does not change what they are looking at, so FindInteractables is not otherwise called
        if (targetInteractable == null)
        {
            FindInteractables();
            return;
        }

        targetInteractable.TryFindInteraction(currentActionState, out InteractionData _targetInteraction);
        TargetInteractableUpdated?.Invoke(targetInteractable, _targetInteraction);
    }
}
