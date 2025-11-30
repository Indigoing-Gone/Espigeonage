using NUnit.Framework.Internal;
using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Drawable : MonoBehaviour
{
    [Header("Components")]
    private BrushSettings settings;
    private SpriteRenderer spriteRenderer;
    private Texture2D drawingTexture;
    private Sprite drawingSprite;

    [Header("Sprite Settings")]
    [SerializeField] private Vector2Int spriteSize = new(1024, 1024);
    [SerializeField] private Color backgroundColor = new(0, 0, 0, 0);

    private Vector2Int? lastPixelCoord = null;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        //Set up base drawing texture
        drawingTexture = new Texture2D(spriteSize.x, spriteSize.y, TextureFormat.RGBA32, false) 
            { filterMode = FilterMode.Point };

        ApplyBackgroundTexture();

        //Create and apply sprite from texture
        drawingSprite = Sprite.Create(drawingTexture, new Rect(0, 0, spriteSize.x, spriteSize.y), new Vector2(0.5f, 0.5f), spriteSize.x);
        spriteRenderer.sprite = drawingSprite;
    }

    private void OnDisable()
    {
        ApplyBackgroundTexture();
    }

    private void DrawAt(Vector2Int _position, bool _updateTexture = true)
    {
        int _size = settings.brushSize;

        for (int i = -_size / 2; i <= _size / 2; i++)
        {
            for (int j = -_size / 2; j <= _size / 2; j++)
            {
                int _drawX = _position.x + i;
                int _drawY = _position.y + j;

                if (_drawX >= 0 && _drawX < spriteSize.x && _drawY >= 0 && _drawY < spriteSize.y)
                    drawingTexture.SetPixel(_drawX, _drawY, settings.brushColor);
            }
        }

        if (_updateTexture) drawingTexture.Apply();
    }

    private void DrawLine(Vector2Int start, Vector2Int end)
    {
        int dx = Mathf.Abs(end.x - start.x);
        int dy = Mathf.Abs(end.y - start.y);
        int sx = start.x < end.x ? 1 : -1;
        int sy = start.y < end.y ? 1 : -1;
        int err = dx - dy;

        int x = start.x;
        int y = start.y;

        while (true)
        {
            DrawAt(new(x, y), false);
            if (x == end.x && y == end.y) break;
            int e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy;
                x += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                y += sy;
            }
        }
        drawingTexture.Apply();
    }

    private void ApplyBackgroundTexture()
    {
        //Applies background color to the texture
        Color32[] _pixels = new Color32[spriteSize.x * spriteSize.y];
        for (int i = 0; i < _pixels.Length; i++) _pixels[i] = backgroundColor;
        drawingTexture.SetPixels32(_pixels);
        drawingTexture.Apply();
    }

    public void Draw(Vector3 _drawPosition, BrushSettings _brushSettings)
    {
        settings = _brushSettings;

        Vector3 _worldPos = Camera.main.ScreenToWorldPoint(_drawPosition);
        Vector3 _localPos = transform.InverseTransformPoint(_worldPos);

        Vector2 _spriteWorldSize = drawingSprite.bounds.size;
        Vector2 _pixelsPerUnit = new(spriteSize.x / _spriteWorldSize.x, spriteSize.y / _spriteWorldSize.y);
        int pixelX = (int)((_localPos.x + _spriteWorldSize.x / 2) * _pixelsPerUnit.x);
        int pixelY = (int)((_localPos.y + _spriteWorldSize.y / 2) * _pixelsPerUnit.y);
        Vector2Int currentPixel = new(pixelX, pixelY);

        if (lastPixelCoord == null)
        {
            DrawAt(currentPixel);
            lastPixelCoord = currentPixel;
        }
        else if (lastPixelCoord != currentPixel)
        {
            DrawLine(lastPixelCoord.Value, currentPixel);
            lastPixelCoord = currentPixel;
        }

        //Debug.Log($"{_drawPosition}, {_worldPos}");
    }

    public void EndDraw()
    {
        lastPixelCoord = null;
    }
}
