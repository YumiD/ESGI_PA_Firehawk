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
                    case (int)AIconChoice.IconChoice.Flat:
                        terrainGrid.AddCellZ((Vector2Int)gridPos, choicesPrefab[choice].prefab);
                        break;
                    case (int)AIconChoice.IconChoice.Slide:
                        ManageSlideBloc(cellMouseIsOver, choicesPrefab);
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
        
        private void ManageSlideBloc(GridCell cellMouseIsOver, IReadOnlyList<IconPrefab> choicesPrefab)
        {
            int z = terrainGrid.GetGridCellActualZ((Vector2Int)cellMouseIsOver.GridPosition);

            GridCell topCell = terrainGrid.GetCell(cellMouseIsOver.GridPosition.x, cellMouseIsOver.GridPosition.y, z);

            GameObject prefab = topCell.Surface == GridCell.CellSurface.Flat
                ? choicesPrefab[(int)AIconChoice.IconChoice.Slide].prefab
                : choicesPrefab[(int)AIconChoice.IconChoice.Flat].prefab;

            terrainGrid.ChangeCellZ((Vector2Int)cellMouseIsOver.GridPosition, prefab);
        }
    }
}