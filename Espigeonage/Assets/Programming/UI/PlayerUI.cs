using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private RawImage cursorImage;
    private RectTransform cursorTransform;
    private RectTransform cursorCanvasTransform;

    [SerializeField] private GameObject interactTooltip;
    [SerializeField] private GameObject secondaryTooltip;
    private TextMeshProUGUI interactTooltipText;
    private TextMeshProUGUI secondaryTooltipText;

    [Header("Display Data")]
    [SerializeField] private Texture2D[] cursorTextures;

    private void Awake()
    {
        cursorTransform = cursorImage.rectTransform;
        cursorCanvasTransform = cursorTransform.parent.GetComponent<RectTransform>();
        cursorTransform.parent.GetComponent<Canvas>().worldCamera = Camera.main;
        Cursor.visible = false;

        interactTooltipText = interactTooltip.GetComponentInChildren<TextMeshProUGUI>();
        secondaryTooltipText = secondaryTooltip.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetCursorPosition(Vector2 _position)
    {
        if(cursorCanvasTransform == null) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(cursorCanvasTransform, _position, Camera.main, out var _cursorPosition);
        cursorTransform.anchoredPosition = _cursorPosition;
    }

    private void SetCursorVisual(CursorType _cursorType) => cursorImage.texture = cursorTextures[(int)_cursorType];

    public void SetDefaultUI()
    {
        SetCursorVisual(CursorType.Point);
        interactTooltipText.text = "DEFAULT";

        interactTooltip.SetActive(false);
    }

    public void ApplyDisplayData(InteractionData _interaction)
    {
        SetCursorVisual(_interaction.cursorType);
        interactTooltipText.text = _interaction.tooltip;

        interactTooltip.SetActive(true);
    }

    public void SetSecondaryTooltip(string _tooltip, bool _enabled)
    {
        secondaryTooltipText.text = _tooltip;
        secondaryTooltip.SetActive(_enabled);
    }
}
