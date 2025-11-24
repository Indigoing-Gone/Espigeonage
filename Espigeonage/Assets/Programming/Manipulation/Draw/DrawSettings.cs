using UnityEngine;

[CreateAssetMenu(menuName = "Interactions/DrawSettings")]
public class DrawSettings : ScriptableObject
{
    [Header("Brush")]
    public int brushSize = 5;
    public Color brushColor = Color.black;
}
