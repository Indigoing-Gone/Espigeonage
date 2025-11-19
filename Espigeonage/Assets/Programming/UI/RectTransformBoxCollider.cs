using UnityEngine;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(BoxCollider))]
public class RectTransformBoxCollider : MonoBehaviour
{
    private float thickness = 0.1f;

    void Reset()
    {
        Sync();
    }

    void OnValidate()
    {
        Sync();
    }

    public void Sync()
    {
        Rect _rect = GetComponent<RectTransform>().rect;
        BoxCollider _col = GetComponent<BoxCollider>();

        _col.size = new(_rect.width, _rect.height, thickness);
        _col.center = Vector3.zero;
    }
}