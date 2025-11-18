using UnityEngine;
using UnityEngine.EventSystems;

public class Dragger : MonoBehaviour
{
    [Header("Components")]
    protected Rigidbody dragPointRb;
    protected IDraggable currentDraggable;

    [Header("Drag Parameters")]
    [SerializeField] protected float dragOffset;
    protected float dragDistance;
    public bool DragOverUI {  get; private set; }
    public bool HasDraggable => currentDraggable != null;

    private void Awake()
    {
        GameObject _dragPoint = new("DragPoint");
        dragPointRb = _dragPoint.AddComponent<Rigidbody>();
        dragPointRb.isKinematic = true;
    }

    public void SetDraggable(IDraggable _draggable, float _draggableDistance)
    {
        if (currentDraggable != null) return;
        dragDistance = _draggableDistance - dragOffset;
        currentDraggable = _draggable;
    }

    public void UpdateDragPosition(Vector3 _newPosition)
    {
        Vector3 _screenPosition = _newPosition + (Vector3.forward * dragDistance);
        Vector3 _worldPosition = Camera.main.ScreenToWorldPoint(_screenPosition);
        dragPointRb.gameObject.transform.position = _worldPosition;
    }

    public virtual void Drag()
    {
        currentDraggable?.Drag(this, dragPointRb);
    }

    public virtual void Release()
    {
        currentDraggable?.Release();
        currentDraggable = null;
    }
}
