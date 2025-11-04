using System;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    [Serializable]
    struct Interaction
    {
        public GameState state;
        public InteractBehaviour behaviour;
    }

    [SerializeField] List<Interaction> interactions = new();
    Dictionary<GameState, InteractBehaviour> interactionDict;

    private void Awake()
    {
        interactionDict = new Dictionary<GameState, InteractBehaviour>();
        foreach (Interaction interaction in interactions)
            if (interaction.behaviour) interactionDict[interaction.state] = interaction.behaviour;
    }

    public void Interact(Interactor _interactor)
    {
        if (!interactionDict.TryGetValue(_interactor.CurrentState, out InteractBehaviour _behaviour)) return;
        _behaviour.Execute(this, _interactor);
    }
}
