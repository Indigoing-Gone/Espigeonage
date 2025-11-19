using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum CursorType
{
    Default = 0,
    Point = 1,
    Grab = 2
}

public class PlayerUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private RawImage cursorImage;
    private RectTransform cursorTransform;
    private RectTransform cursorCanvasTransform;
    [SerializeField] private TextMeshProUGUI tooltipText;

    [Header("Display Data")]
    [SerializeField] private string[] defaultTooltips;
    [SerializeField] private Texture2D[] cursorTextures;

    private void Awake()
    {
        cursorTransform = cursorImage.rectTransform;
        cursorCanvasTransform = cursorTransform.parent.GetComponent<RectTransform>();
        Cursor.visible = false;
    }

    public void SetCursorPosition(Vector2 _position)
    {
        if(cursorCanvasTransform == null) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(cursorCanvasTransform, _position, Camera.main, out var _cursorPosition);
        cursorTransform.anchoredPosition = _cursorPosition;
    }

    public void SetDefaultUI()
    {
        ApplyDisplayData();
        tooltipText.enabled = false;
    }

    public void ApplyDisplayData()
    {

    }
}
