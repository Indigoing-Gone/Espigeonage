using UnityEngine;

[CreateAssetMenu(menuName = "Interactions/Behaviours/Draw")]
public class DrawBehaviour : InteractionBehaviour
{
    public override void Execute(MonoBehaviour _interactable, Interactor _interactor)
    {
        _interactable.gameObject.TryGetComponent<Drawable>(out Drawable _drawable);
        _interactor.gameObject.TryGetComponent<Drawer>(out Drawer _drawer);
        if (_drawable == null || _drawer == null) return;

        _drawer.SetDrawable(_drawable, Camera.main.WorldToScreenPoint(_drawable.transform.position).z);
    }
}
