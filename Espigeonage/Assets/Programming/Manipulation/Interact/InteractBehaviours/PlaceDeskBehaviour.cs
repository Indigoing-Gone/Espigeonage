using UnityEngine;

[CreateAssetMenu(menuName = "Interactions/PlaceDesk")]
public class PlaceDeskBehaviour : InteractBehaviour
{
    public override void Execute(MonoBehaviour _interactable, Interactor _interactor)
    {
        _interactable.gameObject.TryGetComponent<Desk>(out Desk _desk);
        _interactor.gameObject.TryGetComponent<Grabber>(out Grabber _grabber);

        if (!_grabber.HasGrabbable) return;

        IGrabbable _grabbable = _grabber.Release();
        _grabbable.Move(_desk.DropLocation.position);
    }
}
