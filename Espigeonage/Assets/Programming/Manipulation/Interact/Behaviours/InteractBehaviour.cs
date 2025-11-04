using UnityEngine;

public abstract class InteractBehaviour : ScriptableObject
{
    public abstract void Execute(MonoBehaviour _interactable, Interactor _interactor);
}
