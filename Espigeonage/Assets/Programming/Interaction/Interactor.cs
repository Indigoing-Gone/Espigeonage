using UnityEngine;

public abstract class Interactor : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] protected LayerMask interactLayer;

    protected bool isInteracting;
    protected bool canInteract;

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
            HandleInteract();
            isInteracting = true;
        }

        if (!_state) isInteracting = false;
    }
}
