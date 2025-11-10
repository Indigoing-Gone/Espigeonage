using UnityEngine;

public class DrawPlane : MonoBehaviour
{
    [SerializeField] private Vector2Int totalPixels = new(512, 512);
    [SerializeField] private int brushSize = 6;
    private Color brushColor;

    [SerializeField] private Material material;
    [SerializeField] private Texture2D generatedTexture;
    private Color[] colorMap;

    private void Start()
    {
        colorMap = new Color[totalPixels.x * totalPixels.y];
        generatedTexture = new(totalPixels.y, totalPixels.x, TextureFormat.RGBA32, false) { filterMode = FilterMode.Point };
        material.SetTexture("_DrawMap", generatedTexture);

    }
}
