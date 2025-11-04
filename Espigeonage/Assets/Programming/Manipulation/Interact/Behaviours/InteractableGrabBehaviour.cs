using UnityEngine;

[CreateAssetMenu(menuName = "Interactions/InteractableGrab")]
public class InteractableGrabBehaviour : InteractBehaviour
{
    public override void Execute(MonoBehaviour _interactable, Interactor _interactor)
    {
        _interactable.gameObject.TryGetComponent<Grabber>(out Grabber _grabbable);
        _interactor.gameObject.TryGetComponent<Grabber>(out Grabber _grabber);
        if (_grabbable == null || _grabbable == null) return;

        if (_grabbable.CurrentGrabbable != null) return;

        _grabbable.HandleGrab(_grabber.CurrentGrabbable);
    }
}
