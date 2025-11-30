using UnityEngine;

[CreateAssetMenu(menuName = "Interactions/BrushSettings")]
public class BrushSettings : ScriptableObject
{
    [Header("Brush")]
    public int brushSize = 5;
    public Color brushColor = Color.black;
}
