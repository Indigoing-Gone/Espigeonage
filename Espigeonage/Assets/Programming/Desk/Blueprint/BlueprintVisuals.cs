using System.Collections.Generic;
using UnityEngine;

public class BlueprintVisuals : MonoBehaviour
{
    [Header("Components")]
    private BlueprintData data;
    [SerializeField] private Transform spy;
    private LineRenderer line;

    [Header("Grid")]
    [SerializeField] private Vector2Int gridSize;
    [SerializeField] private Vector2 cellSize;
    [SerializeField] private Vector3 origin;

    [Header("Spy")]
    [SerializeField] private Vector2Int startPos;
    [SerializeField] private float spyScale = 0.6f;
    [SerializeField] private List<Vector3> linePositions;

    private void OnEnable()
    {
        data.PathContinued += AddLinePosition;
        data.PathRemoved += RemoveLinePosition;
    }

    private void OnDisable()
    {
        data.PathContinued += AddLinePosition;
        data.PathRemoved += RemoveLinePosition;
    }

    private void Awake()
    {
        data = GetComponent<BlueprintData>();
        line = GetComponentInChildren<LineRenderer>();

        data.Init(gridSize, startPos);
        spy.localScale = cellSize * spyScale;
    }

    private void Start()
    {
        AddLinePosition(startPos);
    }

    private void AddLinePosition(Vector2Int _coord)
    {
        Vector3 _worldPosition = GetWorldPosition(_coord.x, _coord.y) +
            new Vector3(cellSize.x / 2.0f, 0, cellSize.y / 2.0f);
        linePositions.Add(transform.InverseTransformDirection(_worldPosition) - transform.position);
        line.positionCount++;
        UpdateVisuals();
    }

    private void RemoveLinePosition()
    {
        linePositions.RemoveAt(linePositions.Count - 1);
        line.positionCount--;
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        spy.position = GetWorldPosition(data.SpyPosition.x, data.SpyPosition.y) + 
            new Vector3(cellSize.x / 2.0f, 0, cellSize.y / 2.0f);
        spy.eulerAngles = new(90, data.GetSpyRotation(), 0);

        line.SetPositions(linePositions.ToArray());
    }

    private void OnDrawGizmos()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1));
                Gizmos.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y));
            }
        }
        Gizmos.DrawLine(GetWorldPosition(0, gridSize.y), GetWorldPosition(gridSize.x, gridSize.y));
        Gizmos.DrawLine(GetWorldPosition(gridSize.x, 0), GetWorldPosition(gridSize.x, gridSize.y));
    }

    private Vector3 GetWorldPosition(int _x, int _y)
    {
        Vector2 _coords = Vector3.Scale(new(_x, _y), cellSize);
        Vector3 _rotatedCoords = new(_coords.x, 0.01f, _coords.y);
        Vector3 _origin = transform.position - new Vector3(gridSize.x * cellSize.x / 2.0f, 0, gridSize.y * cellSize.y / 2.0f);
        return _origin + _rotatedCoords;
    }
}
