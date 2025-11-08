using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(menuName = "Interactions/Drag")]
public class DragBehaviour : InteractBehaviour
{
    public override void Execute(MonoBehaviour _interactable, Interactor _interactor)
    {
        _interactable.gameObject.TryGetComponent<IDraggable>(out IDraggable _draggable);
        _interactor.gameObject.TryGetComponent<Dragger>(out Dragger _dragger);
        if (_draggable == null || _dragger == null || _dragger.DragOverUI) return;

        _dragger.SetDraggable(_draggable, Camera.main.WorldToScreenPoint(_interactable.transform.position).z);
    }
}
