using UnityEngine;

public class Draggable : MonoBehaviour, IDraggable
{
    private void Awake()
    {

    }

    public bool Drag(Dragger _dragger, Vector3 _inputPosition)
    {
        if (_dragger == null) return false;

        Vector3 _screenPosition = _inputPosition + 
            (Vector3.forward * Camera.main.WorldToScreenPoint(transform.position).z);
        Vector3 _worldPosition = Camera.main.ScreenToWorldPoint(_screenPosition);
        transform.position = _worldPosition;

        return true;
    }

    public void Release()
    {

    }
}
