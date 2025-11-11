using UnityEngine;

[CreateAssetMenu(menuName = "Interactions/PlaceDesk")]
public class PlaceDeskBehaviour : InteractBehaviour
{
    public override void Execute(MonoBehaviour _interactable, Interactor _interactor)
    {
        _interactable.gameObject.TryGetComponent<Desk>(out Desk _desk);
        _interactor.gameObject.TryGetComponent<Grabber>(out Grabber _grabber);
        if (_desk == null || _grabber == null || !_grabber.HasGrabbable) return;

        MonoBehaviour _grabbableObject = _grabber.CurrentGrabbable as MonoBehaviour;
        if (_grabbableObject == null) return;

        _grabbableObject.TryGetComponent<IDraggable>(out IDraggable _draggable);
        if (_draggable == null) return;

        IGrabbable _grabbable = _grabber.Release();
        _grabbable.SetTransform(_desk.DropLocation.position, Quaternion.identity);

        _grabbableObject.TryGetComponent<BlueprintGrid>(out BlueprintGrid _bluepint);
        if (_bluepint == null) return;
        _bluepint.UnlockModification();
    }
}
