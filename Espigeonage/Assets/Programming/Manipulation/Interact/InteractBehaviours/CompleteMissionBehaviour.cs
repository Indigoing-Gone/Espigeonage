using UnityEngine;

[CreateAssetMenu(menuName = "Interactions/CompleteMission")]
public class CompleteMissionBehaviour : InteractionBehaviour
{
    public override void Execute(MonoBehaviour _interactable, Interactor _interactor)
    {
        Transform _parent = _interactable.transform.parent;
        if (_parent == null) return;
        _parent.TryGetComponent<MissionGrabber>(out MissionGrabber _missionGrabber);
        if (_missionGrabber == null) return;
        _missionGrabber.CompleteMission();
    }
}
