using UnityEngine;

[RequireComponent(typeof(Interactable))]
[RequireComponent(typeof(Grabbable))]
[RequireComponent(typeof(Draggable))]
public class Blueprint : MonoBehaviour
{
    [Header("Components")]
    private BPGrid bpGrid;
    [SerializeField] private GameObject spy;
    private LineRenderer line;

    [Header("Grid")]
    [SerializeField] private Vector2Int gridSize;
    [SerializeField] private Vector2 cellSize;
    [SerializeField] private Vector3 origin;

    [Header("Spy")]
    [SerializeField] private Vector2Int startPos;
    [SerializeField] private float spyScale = 0.6f;


    private void Awake()
    {
        bpGrid = GetComponent<BPGrid>();
        line = GetComponent<LineRenderer>();

        bpGrid.Init(gridSize, startPos);
        spy.transform.localScale = cellSize * spyScale;
    }

    private void Update()
    {
        spy.transform.position = GetWorldPosition(bpGrid.SpyPosition.x, bpGrid.SpyPosition.y) + new Vector3(cellSize.x / 2.0f, 0, cellSize.y / 2.0f);
        spy.transform.eulerAngles = new(90, bpGrid.GetSpyRotation(), 0);
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
