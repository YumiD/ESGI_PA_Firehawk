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

        public override void PutObject(GridCell cellMouseIsOver, int choice, List<IconPrefab> choicesPrefab)
        {
            Vector3Int gridPos = cellMouseIsOver.GridPosition;
            if (Input.GetMouseButtonDown(0) && choice >= 0)
            {
                bool isInInventory = GameManager.Instance.VerifyInInventory(choicesPrefab[choice].scriptableObject);
                if (!isInInventory)
                {
                    return;
                }

                Vector2Int pos = new Vector2Int(gridPos.x, gridPos.y);
                
                if (!TerrainGrid.CanPutObject(pos))
                    return;
                
                FireObjectScriptableObject previousObject = TerrainGrid.RemoveObject(pos);
                if (previousObject != null)
                {
                    GameManager.Instance.AddInInventory(previousObject);
                }

                TerrainGrid.CreateObject(pos, choicesPrefab[choice].scriptableObject);
                GameManager.Instance.RemoveInInventory(choicesPrefab[choice].scriptableObject);

                updateUi.Raise();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                bool result = TerrainGrid.RemoveObject(new Vector2Int(gridPos.x, gridPos.y));
                if (result)
                {
                    GameManager.Instance.AddInInventory(choicesPrefab[choice].scriptableObject);
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