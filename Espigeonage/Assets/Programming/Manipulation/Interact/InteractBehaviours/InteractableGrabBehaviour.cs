using UnityEngine;

[CreateAssetMenu(menuName = "Interactions/InteractableGrab")]
public class InteractableGrabBehaviour : InteractBehaviour
{
    public override void Execute(MonoBehaviour _interactable, Interactor _interactor)
    {
        _interactable.gameObject.TryGetComponent<Grabber>(out Grabber _interactableGrabber);
        _interactor.gameObject.TryGetComponent<Grabber>(out Grabber _interactorGrabber);
        if (_interactableGrabber == null || _interactorGrabber == null) return;

        if (!_interactorGrabber.HasGrabbable) return;
        IGrabbable _grabbable = _interactorGrabber.Release();
        _interactableGrabber.SetGrabbable(_grabbable);
        _interactableGrabber.Grab();
    }
}
