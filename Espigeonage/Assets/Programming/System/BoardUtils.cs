
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class BoardUtils
{
    // Returns if given position is within the bounds of given width and height
    public static bool InBounds(Vector2Int pos, int width, int height)
    {
        return pos[0] >= 0 && pos[1] >= 0 && pos[0] < height && pos[1] < width;
    }

    // Returns if give positions are adjacent (diagonal not included)
    public static bool IsAdjacent(Vector2Int pos1, Vector2Int pos2)
    {
        return Mathf.Abs(pos1.x - pos2.x) + Mathf.Abs(pos1.y - pos2.y) == 1;
    }

    // Returns if given path is a connected path (all points are adjacent to points before and after)
    public static bool IsValidPath(List<Vector2Int> path)
    {
        for (int i = 0; i < path.Count - 1; i++)
        {
            if (!IsAdjacent(path[i], path[i+1])) return false;
        }
        return true;
    }

    // Converts Vector2 to Vector2Int via rounding
    public static Vector2Int TruncateVector2(Vector2 vec)
    {
        return new Vector2Int(Mathf.RoundToInt(vec.x), Mathf.RoundToInt(vec.y));
    }

    // Convert a coordinate in format (x, y) with origin at bottom left
    // to row major coordinate
    public static Vector2Int ToRowMajor(Vector2Int coord, int _height)
    {
        int i = _height - 1 - coord[1];
        int j = coord[0];
        return new Vector2Int(i, j);
    }

    public static Vector2Int ToStandardCoordinate(Vector2Int coord, int _height)
    {
        int x = coord[1];
        int y = _height - 1 - coord[0];
        return new Vector2Int(x, y);
    }
}
