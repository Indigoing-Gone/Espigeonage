using UnityEngine;

[CreateAssetMenu(menuName = "Interactions/Draw")]
public class DrawBehaviour : InteractBehaviour
{
    public override void Execute(MonoBehaviour _interactable, Interactor _interactor)
    {
        _interactable.gameObject.TryGetComponent<DrawPlane>(out DrawPlane _drawPlane);
        _interactor.gameObject.TryGetComponent<PlayerData>(out PlayerData _playerData);

        Debug.Log("DRAW");
    }
}
