using UnityEngine;

[CreateAssetMenu(menuName = "Interactions/EnterDesk")]
public class EnterDeskBehaviour : InteractBehaviour
{
    public override void Execute(MonoBehaviour _interactable, Interactor _interactor)
    {
        _interactable.gameObject.TryGetComponent<Desk>(out Desk _desk);
        _interactor.gameObject.TryGetComponent<PlayerStateMachine>(out PlayerStateMachine _manager);
        if (_desk == null || _manager == null) return;

        _manager.ChangeState(_manager.DeskState.UpdateDesk(_desk));
    }
}
