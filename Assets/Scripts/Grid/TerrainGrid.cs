using Grid;
using Helper;
using UnityEngine;

public class TerrainGrid : MonoBehaviour
{
    [SerializeField] private Vector3Int _size = new Vector3Int(30, 30, 10);

    private Vector3 _gridCellSize = new Vector3(5, 2.5f, 5);

    [SerializeField] private bool _generation = true;

    [SerializeField] private GameObject _flatTerrainPrefab;

    private GridCell[,,] _grid;

    private void Start()
    {
        if (_generation)
            CreateGrid();
        else
            BuildGrid();
    }

    private void CreateGrid()
    {
        _grid = new GridCell[_size.x, _size.y, _size.z];

        //Create Grid
        for (int y = 0; y < _size.y; y++)
        {
            for (int x = 0; x < _size.x; x++)
            {
                //Create GridSpace object for each cell
                CreateGridCell(x, y, 0, _flatTerrainPrefab);
            }
        }
    }

    private void CreateGridCell(int x, int y, int z, GameObject cellPrefab)
    {
        _grid[x, y, z] = Instantiate(cellPrefab, new Vector3(x, z, y).Multiply(_gridCellSize), Quaternion.identity)
            .GetComponent<GridCell>();
        _grid[x, y, z].GridPosition = new Vector3Int(x, y, z);
        _grid[x, y, z].transform.parent = transform;
        _grid[x, y, z].gameObject.name = $"Grid Space ({x} , {y} , {z})";
    }

    private void RemoveGridCell(int x, int y, int z)
    {
        Destroy(_grid[x, y, z].gameObject);
        _grid[x, y, z] = null;
    }

    public GridCell GetCell(int x, int y, int z)
    {
        return _grid[x, y, z];
    }

    // If the grid was always generated, we fill the array _gameGrid with existent gridCells
    public void BuildGrid()
    {
        _grid = new GridCell[_size.x, _size.y, _size.z];

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            Vector3Int gridPos = GetGridPosFromWorld(child.position);

            if (!IsGridPosValid(gridPos))
            {
                Debug.Log($"Object not in range ({gridPos}), please adjust the grid size.");
                continue;
            }

            int x = gridPos.x;
            int y = gridPos.y;
            int z = gridPos.z;

            _grid[x, y, z] = child.gameObject.GetComponent<GridCell>();
            _grid[x, y, z].GridPosition = gridPos;

            if (_grid[x, y, z].Anchor.transform.childCount > 0)
            {
                _grid[x, y, z].Object = _grid[x, y, z].Anchor.transform.GetChild(0).gameObject;
            }
        }
    }

    public bool SetObject(Vector2Int pos, GameObject objectPrefab)
    {
        int x = pos.x;
        int y = pos.y;
        int z = GetGridCellActualZ(pos);

        if (_grid[x, y, z].Object != null)
        {
            return false;
        }

        _grid[x, y, z].Object = Instantiate(objectPrefab, _grid[x, y, z].Anchor.transform);
        return true;
    }

    public bool RemoveObject(Vector2Int pos)
    {
        int x = pos.x;
        int y = pos.y;
        int z = GetGridCellActualZ(pos);

        if (_grid[x, y, z].Object != null)
        {
            Destroy(_grid[x, y, z].Object);
            return true;
        }

        return false;
    }

    public void AddCellZ(Vector2Int pos, GameObject cellPrefab)
    {
        int x = pos.x;
        int y = pos.y;
        int currentZ = GetGridCellActualZ(pos);
        int newZ = currentZ + 1;

        if (newZ >= _size.y)
            return;

        if (_grid[x, y, currentZ].Object != null)
        {
            Destroy(_grid[x, y, currentZ].Object);
        }

        if (_grid[x, y, currentZ].Surface == GridCell.CellSurface.Slide)
        {
            ChangeCellZ(pos, _flatTerrainPrefab);
        }

        CreateGridCell(x, y, newZ, cellPrefab);
    }

    public void ChangeCellZ(Vector2Int pos, GameObject cellPrefab)
    {
        int x = pos.x;
        int y = pos.y;
        int z = GetGridCellActualZ(pos);

        GameObject cellObject = null;
        if (_grid[x, y, z].Object != null)
        {
            cellObject = _grid[x, y, z].Object;
            _grid[x, y, z].Object = null;
            cellObject.transform.SetParent(transform.root, false);
        }

        RemoveGridCell(x, y, z);

        CreateGridCell(x, y, z, cellPrefab);

        if (cellObject != null)
        {
            _grid[x, y, z].Object = cellObject;
            cellObject.transform.SetParent(_grid[x, y, z].Anchor.transform, false);
        }
    }

    public void RemoveCellZ(Vector2Int pos)
    {
        int x = pos.x;
        int y = pos.y;
        int z = GetGridCellActualZ(pos);

        if (z == 0) // keep lowest cell
            return;

        RemoveGridCell(x, y, z);
    }

    public int GetGridCellActualZ(Vector2Int pos)
    {
        int x = pos.x;
        int y = pos.y;
        int z = 0;

        while (z < _size.z)
        {
            if (_grid[x, y, z] == null)
                break;
            z++;
        }

        return z - 1;
    }

    public Vector3Int GetGridPosFromWorld(Vector3 worldPosition)
    {
        Vector3 localPosition = transform.worldToLocalMatrix *
                                new Vector4(worldPosition.x, worldPosition.y, worldPosition.z, 1);

        Vector3 zUpGridCellSize = new Vector3
        {
            x = _gridCellSize.x,
            y = _gridCellSize.z,
            z = _gridCellSize.y
        };

        Vector3Int gridPos = new Vector3(localPosition.x, localPosition.z, localPosition.y).Divide(zUpGridCellSize)
            .FloorToInt();

        return gridPos;
    }

    public bool IsGridPosValid(Vector3Int gridPos)
    {
        return gridPos.IsBetween(new Vector3Int(0, 0, 0), _size - new Vector3Int(1, 1, 1));
    }

    public Vector3 GetWorldPosFromGridPos(Vector3Int gridPos)
    {
        Vector3 localPosition = new Vector3(gridPos.x, gridPos.z, gridPos.y).Multiply(_gridCellSize);

        return transform.localToWorldMatrix * new Vector4(localPosition.x, localPosition.y, localPosition.z, 1);
    }
}