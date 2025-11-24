using System.Collections;
using UnityEngine;

public class Drawer : MonoBehaviour
{
    [Header("Components")]
    protected Drawable currentDrawable = null;

    [Header("Draw Parameters")]
    private float drawDistance;
    private Vector3 drawPosition;
    [SerializeField] private float drawDelay = 0;
    public bool HasDrawable => currentDrawable != null;

    public void SetDrawable(Drawable _drawable, float _drawableDistance)
    {
        if (currentDrawable != null) return;
        drawDistance = _drawableDistance;
        currentDrawable = _drawable;
    }

    public void StartDrawing()
    {
        if (currentDrawable == null) return;
        StartCoroutine(UpdateDrawing());
    }

    public void StopDrawing()
    {
        if (currentDrawable == null) return;
        StopAllCoroutines();
        currentDrawable.EndDraw();
        currentDrawable = null;
    }

    private IEnumerator UpdateDrawing()
    {
        currentDrawable.Draw(drawPosition + (Vector3.forward * drawDistance));
        yield return new WaitForSeconds(drawDelay);
        StartCoroutine(UpdateDrawing());
    }

    public void UpdateDrawPosition(Vector3 _drawPosition)
    {
        drawPosition = _drawPosition;
    }
}
