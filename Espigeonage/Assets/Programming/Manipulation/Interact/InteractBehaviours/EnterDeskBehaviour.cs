using UnityEngine;

[CreateAssetMenu(menuName = "Interactions/Behaviours/EnterDesk")]
public class EnterDeskBehaviour : InteractionBehaviour
{
    public override void Execute(MonoBehaviour _interactable, Interactor _interactor)
    {
        _interactable.gameObject.TryGetComponent<Desk>(out Desk _desk);
        _interactor.gameObject.TryGetComponent<PlayerData>(out PlayerData _ctx);
        if (_desk == null || _ctx == null) return;

        _ctx.SetCurrentDesk(_desk);
    }

    public override bool Verify(MonoBehaviour _interactable, Interactor _interactor) => true;
}
