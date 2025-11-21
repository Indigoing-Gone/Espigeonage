using UnityEngine;

public abstract class InteractionBehaviour : ScriptableObject
{
    public abstract void Execute(MonoBehaviour _interactable, Interactor _interactor);
}
