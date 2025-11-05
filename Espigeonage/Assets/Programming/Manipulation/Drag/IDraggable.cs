using UnityEngine;

public interface IDraggable
{
    public bool Drag(Dragger _dragger, Vector3 _inputPosition);
    public void Release();
}
