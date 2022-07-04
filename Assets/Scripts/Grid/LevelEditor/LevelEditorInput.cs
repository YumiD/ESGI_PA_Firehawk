using System;
using System.Collections.Generic;
using Grid.Models;
using UI.Models;
using UnityEngine;

namespace Grid.LevelEditor
{
    public class LevelEditorInput : AInput
    {
        private bool _clicked;

        public override void PutObject(GridCell cellMouseIsOver, int choice, List<ButtonPrefab> choicesPrefab)
        {
            Vector3Int gridPos = cellMouseIsOver.GridPosition;
            if (Input.GetMouseButton(0) && choice >= 0)
            {
                switch (choice)
                {
                    case (int)LevelEditorIconChoice.IconChoice.Flat:
                        _clicked = true;
                        break;
                    case (int)LevelEditorIconChoice.IconChoice.Slide:
                        _clicked = true;
                        break;
                    default:
                        Vector2Int pos = new Vector2Int(gridPos.x, gridPos.y);
                        terrainGrid.SetObject(new Vector2Int(gridPos.x, gridPos.y),
                            choicesPrefab[choice].prefab);
                        terrainGrid.RemoveObject(pos);
                        terrainGrid.CreateObject(choicesPrefab[choice].prefab, pos.x, pos.y);
                        break;
                }
            }
            if (Input.GetMouseButtonUp(0) && _clicked)
            {
                switch (choice)
                {
                    case (int)LevelEditorIconChoice.IconChoice.Flat:
                        terrainGrid.AddCellZ((Vector2Int)gridPos, choicesPrefab[choice].prefab);
                        break;
                    case (int)LevelEditorIconChoice.IconChoice.Slide:
                        ManageSlideBloc(cellMouseIsOver, choicesPrefab);
                        break;
                    case (int)LevelEditorIconChoice.IconChoice.Default:
                    case (int)LevelEditorIconChoice.IconChoice.Tree:
                    case (int)LevelEditorIconChoice.IconChoice.Grass:
                    case (int)LevelEditorIconChoice.IconChoice.Campfire:
                    case (int)LevelEditorIconChoice.IconChoice.Goal:
                    case (int)LevelEditorIconChoice.IconChoice.Palmer:
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
        
        private void ManageSlideBloc(GridCell cellMouseIsOver, IReadOnlyList<ButtonPrefab> choicesPrefab)
        {
            int z = terrainGrid.GetGridCellActualZ((Vector2Int)cellMouseIsOver.GridPosition);

            GridCell topCell = terrainGrid.GetCell(cellMouseIsOver.GridPosition.x, cellMouseIsOver.GridPosition.y, z);

            GameObject prefab = topCell.Surface == GridCell.CellSurface.Flat
                ? choicesPrefab[(int)LevelEditorIconChoice.IconChoice.Slide].prefab
                : choicesPrefab[(int)LevelEditorIconChoice.IconChoice.Flat].prefab;

            terrainGrid.ChangeCellZ((Vector2Int)cellMouseIsOver.GridPosition, prefab);
        }
    }
}