using System;
using System.Collections.Generic;
using System.Linq;
using Helper;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace Grid
{
    public class TerrainGrid : MonoBehaviour
    {
        public Vector3Int Size { get; private set; }

        public Vector3 GridCellSize = new Vector3(5, 2.5f, 5);

        [SerializeField] private GameObject _flatTerrainPrefab;
        [SerializeField] private GameObject _slideTerrainPrefab;

        private GridCell[,,] _grid;

        
        private GameObject _pivot;

        private void CreateGridCell(int x, int y, int z, float angle, GridCell.CellSurface cellType)
        {
            GameObject cellPrefab = cellType switch
            {
                GridCell.CellSurface.Flat => _flatTerrainPrefab,
                GridCell.CellSurface.Slide => _slideTerrainPrefab,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            _grid[x, y, z] = Instantiate(cellPrefab, transform).GetComponent<GridCell>();
            _grid[x, y, z].transform.localPosition = new Vector3(x, z, y).Multiply(GridCellSize);
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

            FireObject objectOnGrid = _grid[x, y, z].Object;
            if (objectOnGrid != null)
            {
                // If object has Removable component, check if it's removable : prevent replacing level props
                return objectOnGrid.Instance.GetComponent<Removable>().IsRemovable;
            }
            return true;
        }

        public void CreateObject(Vector2Int pos, FireObjectScriptableObject scriptableObject, bool makeNonRemovable = false)
        {
            int x = pos.x;
            int y = pos.y;
            int z = GetGridCellActualZ(pos);

            GameObject obj = Instantiate(scriptableObject.Prefab, _grid[x, y, z].Anchor.transform);
            
            _grid[x, y, z].Object = new FireObject
            {
                Instance = obj,
                ScriptableObject = scriptableObject
            };

            obj.AddComponent<Removable>().IsRemovable = makeNonRemovable;
        }

        public FireObjectScriptableObject RemoveObject(Vector2Int pos)
        {
            int x = pos.x;
            int y = pos.y;
            int z = GetGridCellActualZ(pos);

            FireObject fireObject = _grid[x, y, z].Object;
            if (fireObject != null)
            {
                Destroy(fireObject.Instance);
                _grid[x, y, z].Object = null;
                return fireObject.ScriptableObject;
            }

            return null;
        }

        public void AddCellZ(Vector2Int pos, GridCell.CellSurface cellType)
        {
            int x = pos.x;
            int y = pos.y;
            int currentZ = GetGridCellActualZ(pos);
            int newZ = currentZ + 1;

            if (newZ >= Size.y)
                return;

            if (currentZ >= 0)
            {
                if (_grid[x, y, currentZ].Object != null)
                {
                    Destroy(_grid[x, y, currentZ].Object.Instance);
                    _grid[x, y, currentZ].Object = null;
                }

                if (_grid[x, y, currentZ].Surface == GridCell.CellSurface.Slide)
                {
                    ChangeCellZ(pos, GridCell.CellSurface.Flat);
                }
            }

            CreateGridCell(x, y, newZ, 0, cellType);
        }

        public void ChangeCellZ(Vector2Int pos, GridCell.CellSurface cellType)
        {
            int x = pos.x;
            int y = pos.y;
            int z = GetGridCellActualZ(pos);

            float angle = _grid[x, y, z].transform.eulerAngles.y;

            FireObject cellObject = null;
            if (_grid[x, y, z].Object != null)
            {
                cellObject = _grid[x, y, z].Object;
                _grid[x, y, z].Object = null;
                cellObject.Instance.transform.SetParent(transform.root, false);
            }

            RemoveGridCell(x, y, z);

            CreateGridCell(x, y, z, angle, cellType);

            if (cellObject != null)
            {
                _grid[x, y, z].Object = cellObject;
                cellObject.Instance.transform.SetParent(_grid[x, y, z].Anchor.transform, false);
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

            while (z < Size.z)
            {
                if (_grid[x, y, z] == null)
                    break;
                z++;
            }

            return z - 1;
        }

        public bool IsGridPosValid(Vector3Int gridPos)
        {
            return gridPos.IsBetween(new Vector3Int(0, 0, 0), Size - new Vector3Int(1, 1, 1));
        }

        public Vector3 GetCenterGrid(){
            Vector3Int centerGrid = _grid[Size.x/2, Size.y/2, 0].GridPosition * (int)GridCellSize.x;
            Vector3 centerPos = new Vector3();
            centerPos.x = centerGrid.x - (int)GridCellSize.x/2;
            centerPos.y = centerGrid.z - (int)GridCellSize.x/2;
            centerPos.z = centerGrid.y - (int)GridCellSize.x/2;
            return centerPos;
        }
        
        public JObject Serialize()
        {
            JObject root = new JObject();

            root["grid_size"] = Size.ToJson();

            JArray jsonGridCells = new JArray();
            for (int x = 0; x < Size.x; x++)
            {
                for (int y = 0; y < Size.y; y++)
                {
                    for (int z = 0; z < Size.z; z++)
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
                            jsonCell["o"] = gridCell.Object.ScriptableObject.SerializedName;
                        }
						
                        jsonGridCells.Add(jsonCell);
                    }
                }
            }

            root["grid_cells"] = jsonGridCells;

            return root;
        }
        
        public void Deserialize(JObject jsonData, bool makeObjectsNonRemovable = false)
        {
            Size = jsonData["grid_size"].ToVector3Int();
            _grid = new GridCell[Size.x, Size.y, Size.z];
            
            List<FireObjectScriptableObject> scriptableObjects = AssetDatabase.FindAssets($"t: {nameof(FireObjectScriptableObject)}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<FireObjectScriptableObject>)
                .ToList();

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

                string cellTypeStr = (string)jsonCell["t"];
                GridCell.CellSurface cellType = cellTypeStr switch
                {
                    "f" => GridCell.CellSurface.Flat,
                    "s" => GridCell.CellSurface.Slide,
                    _ => throw new ArgumentOutOfRangeException()
                };
				
                CreateGridCell(x, y, z, (float)jsonCell["r"], cellType);

                if (jsonCell.ContainsKey("o"))
                {
                    string objectSerializedName = (string)jsonCell["o"];
                    CreateObject(new Vector2Int(x, y), scriptableObjects.Find(so => so.SerializedName == objectSerializedName), makeObjectsNonRemovable);
                }
            }
        }

        public void Create(Vector3Int size)
        {
            Size = size;
            _grid = new GridCell[Size.x, Size.y, Size.z];
            
            for (int y = 0; y < Size.y; y++)
            {
                for (int x = 0; x < Size.x; x++)
                {
                    CreateGridCell(x, y, 0, 0, GridCell.CellSurface.Flat);
                }
            }
        }
    }
}