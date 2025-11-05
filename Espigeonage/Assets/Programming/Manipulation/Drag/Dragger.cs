using UnityEngine;

public class Dragger : MonoBehaviour
{
    private Vector3 dragPosition;
    [SerializeField] protected IDraggable currentDraggable;
    public IDraggable CurrentDraggable => currentDraggable;

    public virtual void HandleDrag(IDraggable _draggable)
    {
        if (currentDraggable != null || currentDraggable == _draggable) return;
        currentDraggable = _draggable;
    }

    public virtual void HandleRelease()
    {
        currentDraggable = null;
    }

    public virtual void AttemptDrag(Vector3 _position)
    {
        if (currentDraggable == null) return;
        currentDraggable.Drag(this, _position);
    }
}
