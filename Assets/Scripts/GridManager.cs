using UnityEngine;

public class GridManager : MonoBehaviour
{
    public Vector2 gridSize = new Vector2(7, 10);
    public float cellSize = 1f;
    public Transform middleArea;

    public bool[,] grid;

    private void Awake()
    {
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        grid = new bool[(int)gridSize.x, (int)gridSize.y];
    }

    public void OccupyPosition(Vector2 position)
    {
        int x = Mathf.FloorToInt(position.x / cellSize);
        int y = Mathf.FloorToInt(position.y / cellSize);

        if (x >= 0 && x < gridSize.x && y >= 0 && y < gridSize.y)
        {
            grid[x, y] = true;
        }
    }

    public void FreePosition(Vector2 position)
    {
        int x = Mathf.FloorToInt(position.x / cellSize);
        int y = Mathf.FloorToInt(position.y / cellSize);

        if (x >= 0 && x < gridSize.x && y >= 0 && y < gridSize.y)
        {
            grid[x, y] = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (middleArea == null) return;

        Gizmos.color = Color.green;
        Vector3 startPos = middleArea.position;

        for (int x = 0; x <= gridSize.x; x++)
        {
            Vector3 lineStart = startPos + new Vector3(x * cellSize, 0, 0);
            Vector3 lineEnd = lineStart + new Vector3(0, gridSize.y * cellSize, 0);
            Gizmos.DrawLine(lineStart, lineEnd);
        }

        for (int y = 0; y <= gridSize.y; y++)
        {
            Vector3 lineStart = startPos + new Vector3(0, y * cellSize, 0);
            Vector3 lineEnd = lineStart + new Vector3(gridSize.x * cellSize, 0, 0);
            Gizmos.DrawLine(lineStart, lineEnd);
        }

        if (grid != null)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    if (grid[x, y])
                    {
                        Vector3 cellCenter = startPos +
                                             new Vector3(x * cellSize + cellSize / 2, y * cellSize + cellSize / 2, 0);
                        Gizmos.color = Color.red;
                        Gizmos.DrawCube(cellCenter, new Vector3(cellSize, cellSize, 0.1f));
                    }
                }
            }
        }
    }
}