using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Interactions/SwapGrab")]
public class SwapGrabBehaviour : InteractionBehaviour
{
    public override void Execute(MonoBehaviour _interactable, Interactor _interactor)
    {
        _interactable.gameObject.TryGetComponent<IGrabbable>(out IGrabbable _newGrabbable);
        _interactor.gameObject.TryGetComponent<Grabber>(out Grabber _grabber);
        if (_newGrabbable == null || _grabber == null || !_grabber.HasGrabbable) return;

        IGrabbable _oldGrabbable = _grabber.Release();
        AttemptPositionSwap(_newGrabbable, _oldGrabbable);

        _grabber.SetGrabbable(_newGrabbable);
        _grabber.Grab();
    }

    private void AttemptPositionSwap(IGrabbable _newGrabbable, IGrabbable _oldGrabbable)
    {
        //Check if grabbable is object
        MonoBehaviour _newGrabbableObject = _newGrabbable as MonoBehaviour;
        if (_newGrabbableObject == null) return;

        //Check if grabbable has parent
        Transform _newGrabbableParent = _newGrabbableObject.transform.parent;
        if (_newGrabbableParent != null)
        {
            //Check if parent is a Grabber
            _newGrabbableObject.transform.parent.TryGetComponent<Grabber>(out Grabber _newGrabbableGrabber);
            if (_newGrabbableGrabber != null)
            {
                //Grab the old grabbable
                _newGrabbableGrabber.Release();
                _newGrabbableGrabber.SetGrabbable(_oldGrabbable);
                _newGrabbableGrabber.Grab();
                return;
            }
        }

        //Fallback on swapping positions
        _oldGrabbable.SetTransform(_newGrabbableObject.transform.position, _newGrabbableObject.transform.rotation);
    }
}
