using System;
using System.Collections.Generic;
using System.IO;
using Events.Bool;
using Grid.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleFileBrowser;
using UI.Models;
using UnityEngine;

namespace Grid.LevelEditor
{
    public class LevelEditorInput : AInput
    {
        [SerializeField] private EventBool inputEvent;
        private bool _clicked;

        public override void PutObject(GridCell cellMouseIsOver, int choice, List<IconPrefab> choicesPrefab)
        {
            Vector3Int gridPos = cellMouseIsOver.GridPosition;
            if (Input.GetMouseButton(0) && choice >= 0)
            {
                switch (choice)
                {
                    case (int)AIconChoice.IconChoice.Flat:
                        _clicked = true;
                        break;
                    case (int)AIconChoice.IconChoice.Slide:
                        _clicked = true;
                        break;
                    default:
                        Vector2Int pos = new Vector2Int(gridPos.x, gridPos.y);
                        terrainGrid.RemoveObject(pos);
                        terrainGrid.CreateObject(pos, choicesPrefab[choice].scriptableObject);
                        break;
                }
            }
            if (Input.GetMouseButtonUp(0) && _clicked)
            {
                switch (choice)
                {
                    case (int)AIconChoice.IconChoice.Flat:
                        terrainGrid.AddCellZ((Vector2Int)gridPos, GridCell.CellSurface.Flat);
                        break;
                    case (int)AIconChoice.IconChoice.Slide:
                        SwitchCellType(cellMouseIsOver);
                        break;
                    case (int)AIconChoice.IconChoice.Default:
                    case (int)AIconChoice.IconChoice.Tree:
                    case (int)AIconChoice.IconChoice.BombTree:
                    case (int)AIconChoice.IconChoice.Grass:
                    case (int)AIconChoice.IconChoice.Campfire:
                    case (int)AIconChoice.IconChoice.Goal:
                    case (int)AIconChoice.IconChoice.Palmer:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                _clicked = false;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                gridPos = cellMouseIsOver.GridPosition;
                terrainGrid.RemoveCellZ((Vector2Int)gridPos);
            }
        }
        
        private void SwitchCellType(GridCell cellMouseIsOver)
        {
            int z = terrainGrid.GetGridCellActualZ((Vector2Int)cellMouseIsOver.GridPosition);

            GridCell topCell = terrainGrid.GetCell(cellMouseIsOver.GridPosition.x, cellMouseIsOver.GridPosition.y, z);

            GridCell.CellSurface newCellType = topCell.Surface == GridCell.CellSurface.Flat
                ? GridCell.CellSurface.Slide
                : GridCell.CellSurface.Flat;

            terrainGrid.ChangeCellZ((Vector2Int)cellMouseIsOver.GridPosition, newCellType);
        }
        
        public void ExportTerrainJson()
        {
            FileBrowser.SetFilters(false, new FileBrowser.Filter("Level data", ".json"));
            FileBrowser.ShowSaveDialog(path => {
                JObject root = terrainGrid.Serialize();

                File.WriteAllText(path[0], root.ToString(Formatting.None));
            }, () =>
            {
                inputEvent.Raise(true);
            }, FileBrowser.PickMode.Files);
        }
    }
}