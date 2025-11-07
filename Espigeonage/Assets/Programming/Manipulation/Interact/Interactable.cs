using System;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    [Serializable]
    struct Interaction
    {
        public ActionState state;
        public InteractBehaviour behaviour;
    }

    [SerializeField] List<Interaction> interactions = new();
    Dictionary<ActionState, InteractBehaviour> interactionDict;

    private void Awake()
    {
        interactionDict = new Dictionary<ActionState, InteractBehaviour>();
        foreach (Interaction interaction in interactions)
            if (interaction.behaviour) interactionDict[interaction.state] = interaction.behaviour;
    }

    public void Interact(Interactor _interactor, ActionState _currentActionState)
    {
        if (!interactionDict.TryGetValue(_currentActionState, out InteractBehaviour _behaviour)) return;
        _behaviour.Execute(this, _interactor);
    }
}
