using System.Collections.Generic;
using Events.Trigger;
using Grid.Models;
using UI.Models;
using UnityEngine;

namespace Grid.InGame
{
    public class InGameInput : AInput
    {
        [SerializeField] private EventTrigger updateUi;

        public override void PutObject(GridCell cellMouseIsOver, int choice, List<ButtonPrefab> choicesPrefab)
        {
            Vector3Int gridPos = cellMouseIsOver.GridPosition;
            if (Input.GetMouseButtonDown(0) && choice >= 0)
            {
                int index = GameManager.Instance.VerifyInInventory(choicesPrefab[choice].prefab);
                if (index == -1)
                {
                    return;
                }

                Vector2Int pos = new Vector2Int(gridPos.x, gridPos.y);
                (bool, GameObject) result = TerrainGrid.SetObject(pos, choicesPrefab[choice].prefab);
                if (!result.Item1)
                {
                    // If different objects
                    GameManager.Instance.AddInInventory(result.Item2);
                    TerrainGrid.RemoveObject(pos);
                    TerrainGrid.CreateObject(choicesPrefab[choice].prefab, pos.x, pos.y);
                    TerrainGrid.AddInInventory(pos.x, pos.y);
                }
                else
                {
                    GameManager.Instance.RemoveInInventory(index);
                }

                updateUi.Raise();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                bool result = TerrainGrid.RemoveObject(new Vector2Int(gridPos.x, gridPos.y));
                if (result)
                {
                    GameManager.Instance.AddInInventory(choicesPrefab[choice].prefab);
                    updateUi.Raise();
                }
            }
        }

        // public bool SetObject(Vector2Int pos, GameObject objectPrefab)
        // {
        //     int x = pos.x;
        //     int y = pos.y;
        //     int z = GetGridCellActualZ(pos);
        //
        //     if (_grid[x, y, z].Object != null)
        //     {
        //         // If different objects
        //         if (!_grid[x, y, z].Object.name.Contains(objectPrefab.name))
        //         {
        //             GameManager.Instance.AddInInventory(_grid[x, y, z].Object);
        //             Destroy(_grid[x, y, z].Object);
        //             CreateObject(objectPrefab, x, y, z);
        //             return true;
        //         }
        //         return false;
        //     }
        //
        //     CreateObject(objectPrefab, x, y, z);
        //     return true;
        // }
    }
}