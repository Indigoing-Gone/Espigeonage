using UnityEngine;

[CreateAssetMenu(menuName = "Interactions/InteractorGrab")]
public class InteractorGrabBehaviour : InteractBehaviour
{
    public override void Execute(MonoBehaviour _interactable, Interactor _interactor)
    {
        _interactable.gameObject.TryGetComponent<IGrabbable>(out IGrabbable _grabbable);
        _interactor.gameObject.TryGetComponent<Grabber>(out Grabber _grabber);
        if (_grabbable == null || _grabber == null) return;

        _grabber.SetGrabbable(_grabbable);
    }
}
