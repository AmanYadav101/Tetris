using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private BlockController _selectedBlock;
    private GridManager _gridManager;
    private void Awake()
    {
        _gridManager = FindObjectOfType<GridManager>();
        if (_gridManager == null)
        {
            Debug.LogError("GridManager not found in the scene.");
        }
    }

    private void Update()
    {
        HandleInput();
        if (Win())
        {
            Debug.Log("Game Over");
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) 
        {
            SelectBlock();
        }

        if (Input.GetKeyUp(KeyCode.Mouse0)) 
        {
            DropBlock();
        }

        if (_selectedBlock)
        {
            DragBlock();
        }
    }

    private void SelectBlock()
    {
        var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider && hit.collider.CompareTag("Block"))
        {
            _selectedBlock = hit.collider.GetComponent<BlockController>();
            _selectedBlock.OnSelected();
        }
    }

    private void DropBlock()
    {
        if (_selectedBlock)
        {
           
                _selectedBlock.OnDropped();

            _selectedBlock = null; 
        }
    }
    private void DragBlock()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        _selectedBlock.transform.position = mousePos;
    }

    bool Win()
    {
        for (int x = 0; x < _gridManager.gridSize.x; x++)
        {
            for (int y = 0; y < _gridManager.gridSize.y; y++)
            {
                if (!_gridManager.grid[x, y])
                {
                    return false;
                }
            }
        }
        return true;
    }
}