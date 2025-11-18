using UnityEngine;

public class CursorHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private RectTransform cursorTransform;
    private RectTransform cursorCanvasTransform;

    [Header("Cursor Textures")]
    [SerializeField] private Texture2D baseCursorTexture;
    [SerializeField] private Texture2D pointCursorTexture;
    [SerializeField] private Texture2D grabCursorTexture;

    private void Awake()
    {
        cursorCanvasTransform = cursorTransform.parent.GetComponent<RectTransform>();
        Cursor.visible = false;
    }

    public void SetCursorPosition(Vector2 _position)
    {
        if(cursorCanvasTransform == null) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(cursorCanvasTransform, _position, Camera.main, out var _cursorPosition);
        cursorTransform.anchoredPosition = _cursorPosition;
    }
}
