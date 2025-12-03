using UnityEngine;

[CreateAssetMenu(menuName = "Interactions/Behaviours/SendPigeon")]
public class SendPigeonBehavior : InteractionBehaviour
{
    public override void Execute(MonoBehaviour _interactable, Interactor _interactor)
    {
        _interactable.transform.TryGetComponent(out PigeonSender _pigeonSender);
        if (_pigeonSender == null) return;
        _pigeonSender.SendPigeon();
    }

    public override bool Verify(MonoBehaviour _interactable, Interactor _interactor) => true;
}