using System;
using Helper;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Grid
{
    public class TerrainGrid : MonoBehaviour
    {
        [SerializeField] private Vector3Int _size = new Vector3Int(30, 30, 10);

        private Vector3 _gridCellSize = new Vector3(5, 2.5f, 5);

        private bool _initialized;

        [SerializeField] private GameObject _flatTerrainPrefab;
        [SerializeField] private GameObject _slideTerrainPrefab;

        private GridCell[,,] _grid;

        private void Start()
        {
            if (!_initialized)
            {
                CreateGrid();
                _initialized = true;
            }
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
                    CreateGridCell(x, y, 0, 0, _flatTerrainPrefab);
                }
            }
        }

        private void CreateGridCell(int x, int y, int z, float angle, GameObject cellPrefab)
        {
            _grid[x, y, z] = Instantiate(cellPrefab, transform).GetComponent<GridCell>();
            _grid[x, y, z].transform.localPosition = new Vector3(x, z, y).Multiply(_gridCellSize);
            _grid[x, y, z].transform.localRotation = Quaternion.Euler(0, angle, 0);
            _grid[x, y, z].GridPosition = new Vector3Int(x, y, z);
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

        public bool CanPutObject(Vector2Int pos)
        {
            int x = pos.x;
            int y = pos.y;
            int z = GetGridCellActualZ(pos);

            GameObject objectOnGrid = _grid[x, y, z].Object;
            if (objectOnGrid != null)
            {
                // If object has Removable component, check if it's removable : prevent replacing level props
                return !objectOnGrid.TryGetComponent(out Removable removable) || removable.IsRemovable;
            }
            return true;
        }

        public (bool Posable, GameObject newGameobject) SetObject(Vector2Int pos, GameObject objectPrefab)
        {
            int x = pos.x;
            int y = pos.y;
            int z = GetGridCellActualZ(pos);

            GameObject objectOnGrid = _grid[x, y, z].Object;
            if (objectOnGrid != null)
            {
                return (Posable: false, newGameobject: _grid[x, y, z].Object);
            }

            _grid[x, y, z].Object = Instantiate(objectPrefab, _grid[x, y, z].Anchor.transform);
            return (Posable: true, newGameobject: _grid[x, y, z].Object);
        }

        public void CreateObject(GameObject objectPrefab, int x, int y)
        {
            int z = GetGridCellActualZ(new Vector2Int(x, y));
            _grid[x, y, z].Object = Instantiate(objectPrefab, _grid[x, y, z].Anchor.transform);
        }

        public void AddInInventory(int x, int y)
        {
            int z = GetGridCellActualZ(new Vector2Int(x, y));
            int index = GameManager.Instance.VerifyInInventory(_grid[x, y, z].Object);
            GameManager.Instance.RemoveInInventory(index);
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

            if (currentZ >= 0)
            {
                if (_grid[x, y, currentZ].Object != null)
                {
                    Destroy(_grid[x, y, currentZ].Object);
                }

                if (_grid[x, y, currentZ].Surface == GridCell.CellSurface.Slide)
                {
                    ChangeCellZ(pos, _flatTerrainPrefab);
                }
            }

            CreateGridCell(x, y, newZ, 0, cellPrefab);
        }

        public void ChangeCellZ(Vector2Int pos, GameObject cellPrefab)
        {
            int x = pos.x;
            int y = pos.y;
            int z = GetGridCellActualZ(pos);

            float angle = _grid[x, y, z].transform.eulerAngles.y;

            GameObject cellObject = null;
            if (_grid[x, y, z].Object != null)
            {
                cellObject = _grid[x, y, z].Object;
                _grid[x, y, z].Object = null;
                cellObject.transform.SetParent(transform.root, false);
            }

            RemoveGridCell(x, y, z);

            CreateGridCell(x, y, z, angle, cellPrefab);

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

        public Vector3Int GetGridPosFromScene(Vector3 scenePosition)
        {
            Vector3 zUpGridCellSize = new Vector3
            {
                x = _gridCellSize.x,
                y = _gridCellSize.z,
                z = _gridCellSize.y
            };

            return new Vector3(scenePosition.x, scenePosition.z, scenePosition.y).Divide(zUpGridCellSize).FloorToInt();
        }

        public bool IsGridPosValid(Vector3Int gridPos)
        {
            return gridPos.IsBetween(new Vector3Int(0, 0, 0), _size - new Vector3Int(1, 1, 1));
        }
        
        public JObject Serialize()
        {
            JObject root = new JObject();

            root["grid_size"] = _size.ToJson();

            JArray jsonGridCells = new JArray();
            for (int x = 0; x < _size.x; x++)
            {
                for (int y = 0; y < _size.y; y++)
                {
                    for (int z = 0; z < _size.z; z++)
                    {
                        GridCell gridCell = _grid[x, y, z];

                        if (gridCell == null)
                        {
                            continue;
                        }
                        
                        JObject jsonCell = new JObject();

                        jsonCell["p"] = new JArray(x, y, z);
                        jsonCell["r"] = gridCell.transform.eulerAngles.y;

                        jsonCell["t"] = gridCell.Surface switch
                        {
                            GridCell.CellSurface.Flat => "f",
                            GridCell.CellSurface.Slide => "s",
                            _ => throw new ArgumentOutOfRangeException()
                        };

                        if (gridCell.Object != null)
                        {
                            jsonCell["o"] = "";//TODO
                        }
						
                        jsonGridCells.Add(jsonCell);
                    }
                }
            }

            root["grid_cells"] = jsonGridCells;

            return root;
        }
        
        public void Deserialize(JObject jsonData)
        {
            _size = jsonData["grid_size"].ToVector3Int();
            _grid = new GridCell[_size.x, _size.y, _size.z];

            JArray jsonGridCells = (JArray)jsonData["grid_cells"];
            for (int i = 0; i < jsonGridCells.Count; i++)
            {
                JObject jsonCell = (JObject)jsonGridCells[i];

                Vector3Int gridPos = jsonCell["p"].ToVector3Int();

                if (!IsGridPosValid(gridPos))
                {
                    Debug.Log($"Object not in range ({gridPos}), please adjust the grid size.");
                    continue;
                }

                int x = gridPos.x;
                int y = gridPos.y;
                int z = gridPos.z;

                string cellType = (string)jsonCell["t"];
                GameObject prefab = cellType switch
                {
                    "f" => _flatTerrainPrefab,
                    "s" => _slideTerrainPrefab,
                    _ => throw new ArgumentOutOfRangeException()
                };
				
                CreateGridCell(x, y, z, (float)jsonCell["r"], prefab);

                if (jsonCell.ContainsKey("object"))
                {
                    //TODO
                }
            }

            _initialized = true;
        }
    }
}