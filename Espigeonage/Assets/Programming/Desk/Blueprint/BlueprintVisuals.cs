using System.Collections.Generic;
using UnityEngine;

public class BlueprintVisuals : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform spyHead;
    private LineRenderer spyTrail;

    [Header("Grid")]
    private Vector2Int gridSize = Vector2Int.zero;
    [SerializeField] private Vector2 cellSize;
    private Vector3 HalfCellSize => cellSize * 0.5f;

    [Header("Spy")]
    [SerializeField] private float spyScale = 0.6f;
    [SerializeField] private float trailScale = 0.6f;
    [SerializeField] private float visualOffset;
    [SerializeField] private List<Vector3> spyTrailPositions;

    private void Awake()
    {
        spyTrail = GetComponentInChildren<LineRenderer>();
        spyTrail.positionCount = 0;

        spyTrailPositions = new();
        spyHead.localScale = cellSize * spyScale;
        spyTrail.widthMultiplier = Mathf.Min(cellSize.x, cellSize.y) * trailScale;
    }

    public void SetGridSize(Vector2Int _gridSize) => gridSize = _gridSize;

    public void AddTrailPosition(Vector2Int _coord)
    {
        Vector3 _worldPosition = GetLocalPosition(_coord.x, _coord.y) + HalfCellSize;
        spyTrailPositions.Add(_worldPosition);
        spyTrail.positionCount++;
    }

    public void RemoveTrailPosition()
    {
        spyTrailPositions.RemoveAt(spyTrailPositions.Count - 1);
        spyTrail.positionCount--;
    }

    public void UpdateVisuals(Vector2Int _spyCoords, float _spyDirection)
    {
        Vector3 _spyWorldPosition = GetLocalPosition(_spyCoords.x, _spyCoords.y) + HalfCellSize;
        spyHead.localPosition = new(_spyWorldPosition.x, visualOffset, _spyWorldPosition.y);
        spyHead.eulerAngles = new(90, _spyDirection, 0);

        spyTrail.SetPositions(spyTrailPositions.ToArray());
    }

    private Vector3 GetLocalPosition(int _x, int _y)
    {
        Vector3 _coords = Vector3.Scale(new(_x, _y, 0), cellSize);
        Vector3 _origin = Vector3.Scale((Vector2)gridSize, HalfCellSize);
        return _coords - _origin;
    }

    private void OnDrawGizmos()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Gizmos.color = Color.blue;
                Vector3 _start = GetLocalPosition(x, y);
                _start = new Vector3(_start.x, _start.z + visualOffset, _start.y) + transform.position;

                Vector3 _end = GetLocalPosition(x, y + 1);
                _end = new Vector3(_end.x, _end.z + visualOffset, _end.y) + transform.position;
                Gizmos.DrawLine(_start, _end);

                _end = GetLocalPosition(x + 1, y);
                _end = new Vector3(_end.x, _end.z + visualOffset, _end.y) + transform.position;
                Gizmos.DrawLine(_start, _end);
            }
        }
        //Gizmos.DrawLine(GetWorldPosition(0, gridSize.y), GetWorldPosition(gridSize.x, gridSize.y));
        //Gizmos.DrawLine(GetWorldPosition(gridSize.x, 0), GetWorldPosition(gridSize.x, gridSize.y));
    }
}
