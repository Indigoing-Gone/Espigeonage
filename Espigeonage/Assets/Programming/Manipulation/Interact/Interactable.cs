using System;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    [SerializeField] private InteractionSet interactions;

    public void Interact(Interactor _interactor, ActionState _currentActionState)
    {
        if (!TryFindInteraction(_currentActionState, out InteractionData _foundInteraction)) return;
        _foundInteraction.behaviour.Execute(this, _interactor);
    }

    public bool TryFindInteraction(ActionState _requiredState, out InteractionData _inputInteraction)
    {
        bool _result = interactions.TryFindInteraction(_requiredState, out InteractionData _foundInteraction);
        _inputInteraction = _foundInteraction;
        return _result;
    }
}
