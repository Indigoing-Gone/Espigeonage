using System;
using System.Collections.Generic;
using UnityEngine;

public enum CursorType
{
    Point = 0,
    Grab = 1,
    Release = 2
}

[Serializable]
public struct InteractionData
{
    public ActionState requiredState;
    public CursorType cursorType;
    public string tooltip;
    public InteractionBehaviour behaviour;
}

[CreateAssetMenu(menuName = "Interactions/InteractionSet")]
public class InteractionSet : ScriptableObject
{
    [SerializeField] private List<InteractionData> interactions = new();
    private Dictionary<ActionState, InteractionData> interactionDict;

    public void Initilize()
    {
        interactionDict = new Dictionary<ActionState, InteractionData>();
        foreach (InteractionData interaction in interactions)
            interactionDict[interaction.requiredState] = interaction;
    }

    public bool TryFindInteraction(ActionState _requiredState, out InteractionData _inputInteraction)
    {
        if (interactionDict == null) Initilize();

        if (!interactionDict.TryGetValue(_requiredState, out InteractionData _foundInteraction))
        {
            _inputInteraction = default;
            return false;
        }

        _inputInteraction = _foundInteraction;
        return true;
    }
}
