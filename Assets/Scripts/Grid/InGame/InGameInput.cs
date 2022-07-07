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
            Vector2Int pos = new Vector2Int(gridPos.x, gridPos.y);
            if (Input.GetMouseButtonDown(0) && choice >= 0)
            {
                bool isInInventory = GameManager.Instance.VerifyInInventory(choicesPrefab[choice].scriptableObject);
                if (!isInInventory)
                {
                    return;
                }

                
                if (!TerrainGrid.CanPutObject(pos))
                    return;
                
                FireObjectScriptableObject previousObject = TerrainGrid.RemoveObject(pos);
                if (previousObject != null)
                {
                    GameManager.Instance.AddInInventory(previousObject);
                    GameObject previousInstance = TerrainGrid.GetInstanceFromCell(pos);
                    if (!GameManager.Instance.VerifyInInventory(previousObject))
                    {
                        ReserveObjectManager.Instance.RemoveInReserve(previousInstance.transform);
                    }
                }

                GameObject obj = TerrainGrid.CreateObject(pos, choicesPrefab[choice].scriptableObject);
                ReserveObjectManager.Instance.AddInReserve(choicesPrefab[choice].scriptableObject, obj.transform);

                GameManager.Instance.RemoveInInventory(choicesPrefab[choice].scriptableObject);

                updateUi.Raise();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                if (!TerrainGrid.CanPutObject(pos))
                {
                    return;
                }

                GameObject obj = TerrainGrid.GetInstanceFromCell(pos);
                FireObjectScriptableObject result = TerrainGrid.RemoveObject(pos);
                if (result)
                {
                    GameManager.Instance.AddInInventory(choicesPrefab[choice].scriptableObject);
                    ReserveObjectManager.Instance.RemoveInReserve(obj.transform);
                    updateUi.Raise();
                }
            }
        }
    }
}