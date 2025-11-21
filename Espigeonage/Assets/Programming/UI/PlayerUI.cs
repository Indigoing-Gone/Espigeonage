using System;
using TMPro;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private RawImage cursorImage;
    private RectTransform cursorTransform;
    private RectTransform cursorCanvasTransform;
    [SerializeField] private TextMeshProUGUI tooltipText;

    [Header("Display Data")]
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

    private void SetCursorVisual(CursorType _cursorType) => cursorImage.texture = cursorTextures[(int)_cursorType];

    private void SetTooltip(string _tooltip)
    {
        tooltipText.text = _tooltip;
    }

    public void SetDefaultUI()
    {
        SetCursorVisual(CursorType.Point);
        SetTooltip("DEFAULT");
        tooltipText.enabled = false;
    }

    public void ApplyDisplayData(InteractionData _interaction)
    {
        SetCursorVisual(_interaction.cursorType);
        SetTooltip(_interaction.tooltip);
        tooltipText.enabled = true;
    }
}
