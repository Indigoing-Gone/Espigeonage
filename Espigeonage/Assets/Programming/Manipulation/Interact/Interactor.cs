using UnityEngine;

public abstract class Interactor : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] protected LayerMask interactLayer;
    [SerializeField] protected ActionState currentActionState;

    protected bool canInteract;

    protected virtual void Awake()
    {
        canInteract = true;
    }

    protected abstract void AttemptInteract();
    public void TriggerInteraction() { if (canInteract) AttemptInteract(); }
    public void SetActionState(ActionState _newActionState) => currentActionState = _newActionState;
}
