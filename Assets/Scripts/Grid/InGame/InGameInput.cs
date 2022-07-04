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
                    // throw new ArgumentException($"{choicesPrefab[choice].prefab.name} is not in the inventory.");
                }

                bool result = TerrainGrid.SetObject(new Vector2Int(gridPos.x, gridPos.y),
                    choicesPrefab[choice].prefab);
                if (!result) return;
                GameManager.Instance.RemoveInInventory(index);
                updateUi.Raise();
            } else if (Input.GetMouseButtonDown(1))
            {
                bool result = TerrainGrid.RemoveObject(new Vector2Int(gridPos.x, gridPos.y));
                if (result)
                {
                    GameManager.Instance.AddInInventory(choicesPrefab[choice].prefab);
                    updateUi.Raise();
                }
            }
        }
    }
}