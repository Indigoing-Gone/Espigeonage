using UnityEngine;

public abstract class Interactor : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] protected LayerMask interactLayer;
    public GameState CurrentState { get; private set; }

    protected bool isInteracting;
    [SerializeField] protected bool canInteract;

    protected virtual void Awake()
    {
        isInteracting = false;
        canInteract = true;
    }

    protected abstract void HandleInteract();
    public virtual void OnInteractStatus(bool _state)
    {
        if (canInteract && !isInteracting)
        {
            isInteracting = true;
            HandleInteract();
        }

        if (!_state && isInteracting) isInteracting = false;
    }
    public virtual void UpdateState(GameState _newState) => CurrentState = _newState;
}
