using UnityEngine;

public class BlockController : MonoBehaviour
{
    public Vector3 InitialPosition { get; set; }
    private SpriteRenderer _spriteRenderer;

    public GameObject rig; 

    private GridManager _gridManager;
    public bool _isFalling = false;
    private float _fallSpeed = 2f; 
    private Vector3 _targetPosition;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _gridManager = FindObjectOfType<GridManager>();
    }

    public void OnSelected()
    {
        _spriteRenderer.sortingOrder = 51;
        _isFalling = false;
        FreeGridCells();
        
        foreach (Transform subBlock in rig.transform)
        {
            int x = Mathf.FloorToInt(subBlock.position.x / _gridManager.cellSize);
            int y = Mathf.FloorToInt(subBlock.position.y / _gridManager.cellSize);
            _gridManager.MakeBlocksAboveFall(x, y);
        }
    }

    public void OnDropped()
    {
        _spriteRenderer.sortingOrder = 50;
        _isFalling = true;

        SnapToGrid();

        _targetPosition = transform.position;

        if (!CanPlaceBlock())
        {
            ResetToInitialPosition();
            _isFalling = false;
        }
    }

    public void ResetToInitialPosition()
    {
        transform.position = InitialPosition;
        _isFalling = false;
    }

    private void Update()
    {
        if (_isFalling)
        {
            SmoothFallRegister();
        }
    }

    void RegisterBlock()
    {
        if (rig == null) return;

        foreach (Transform subBlock in rig.transform)
        {
            Vector2 subBlockWorldPosition = subBlock.position;
            _gridManager.OccupyPosition(subBlockWorldPosition, this);
        }
    }

    private void SmoothFallRegister()
    {
        if (!rig) return;
        _targetPosition += Vector3.down * _gridManager.cellSize;

        if (!CanFall())
        {
            _targetPosition -= Vector3.down * _gridManager.cellSize;
            RegisterBlock();
            _isFalling = false;
            return;
        }

        transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * _fallSpeed);
        if (Vector3.Distance(transform.position, _targetPosition) < 0.01f)
        {
            transform.position = _targetPosition;
        }
    }

    bool CanFall()
    {
        if (rig == null) return false;

        foreach (Transform subBlock in rig.transform)
        {
            int x = Mathf.FloorToInt(subBlock.position.x / _gridManager.cellSize);
            int y = Mathf.FloorToInt(subBlock.position.y / _gridManager.cellSize) - 1;

            Debug.Log($"Checking sub-block at ({x}, {y})");

            if (x < 0 || x >= _gridManager.gridSize.x || y < 0 || _gridManager.grid[x, y])
            {
                return false;
            }
        }

        return true;
    }

    bool CanPlaceBlock()
    {
        if (!rig) return false;

        foreach (Transform subBlock in rig.transform)
        {
            int x = Mathf.FloorToInt(subBlock.position.x / _gridManager.cellSize);
            int y = Mathf.FloorToInt(subBlock.position.y / _gridManager.cellSize) - 1;

            if (x < 0 || x >= _gridManager.gridSize.x || y < 0 || y >= _gridManager.gridSize.y || _gridManager.grid[x, y])
            {
                return false;
            }
        }

        return true;
    }

    private void SnapToGrid()
    {
        if (_gridManager == null) return;

        float snappedX = Mathf.Round(transform.position.x / _gridManager.cellSize) * _gridManager.cellSize;
        float snappedY = Mathf.Round(transform.position.y / _gridManager.cellSize) * _gridManager.cellSize;

        transform.position = new Vector3(snappedX, snappedY, transform.position.z);
    }

    public void FreeGridCells()
    {
        if (rig == null) return;

        foreach (Transform subBlock in rig.transform)
        {
            Vector2 subBlockWorldPosition = subBlock.position;
            _gridManager.FreePosition(subBlockWorldPosition);
        }
    }
    
    
}