using System;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Grid
{
    public class InputManagerBuild : MonoBehaviour
    {
        private enum IconChoice
        {
            Default = -1,
            TreeBase = 0,
            TreeCoconut = 1,
            Grass = 2,
            Flat = 3,
            Slide = 4,
        }

        private IconChoice _choice;
        [SerializeField] private List<ButtonPrefab> choicesPrefab = new List<ButtonPrefab>();
        private TerrainGrid _terrainGrid;

        [SerializeField] private LayerMask whatIsAGridLayer;

        private Camera _camera;
        private GameObject _lastSelected;
        private bool _clicked;

        private void Start()
        {
            _choice = IconChoice.Default;
            _camera = Camera.main;
            _terrainGrid = FindObjectOfType<TerrainGrid>();
            for (var i = 0; i < choicesPrefab.Count; i++)
            {
                var numChoice = i;
                choicesPrefab[i].iconButton.onClick.AddListener(delegate { SetChoice(numChoice); });
            }
        }

        private void Update()
        {
            ButtonStaySelected();
            var cellMouseIsOver = IsMouseOverAGridSpace();

            if (cellMouseIsOver == null) return;

            var gridPos = cellMouseIsOver.GridPosition;
            if (Input.GetMouseButton(0) && (int)_choice >= 0)
            {
                switch (_choice)
                {
                    case IconChoice.Flat:
                        _clicked = true;
                        break;
                    case IconChoice.Slide:
                        _clicked = true;
                        break;
                    case IconChoice.TreeBase:
                    case IconChoice.TreeCoconut:
                    case IconChoice.Grass:
                    case IconChoice.Default:
                    default:
                        _terrainGrid.SetObject(new Vector2Int(gridPos.x, gridPos.y),
                            choicesPrefab[(int)_choice].prefab);
                        break;
                }
            }
            else if (Input.GetMouseButtonUp(0) && _clicked)
            {
                switch (_choice)
                {
                    case IconChoice.Flat:
                        _terrainGrid.AddCellZ((Vector2Int)gridPos, choicesPrefab[(int)_choice].prefab);
                        break;
                    case IconChoice.Slide:
                        ManageSlideBloc(cellMouseIsOver);
                        break;
                    case IconChoice.Default:
                        break;
                    case IconChoice.TreeBase:
                        break;
                    case IconChoice.TreeCoconut:
                        break;
                    case IconChoice.Grass:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                _clicked = false;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                gridPos = cellMouseIsOver.GridPosition;
                _terrainGrid.RemoveCellZ((Vector2Int)gridPos);
            }


            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SetChoice(0);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SetChoice(1);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SetChoice(2);
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SetChoice(3);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                cellMouseIsOver.transform.Rotate(0, 90, 0);
            }
        }

        // Returns grid cell if mouse is over it, else null
        private GridCell IsMouseOverAGridSpace()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            return Physics.Raycast(ray, out var hitInfo, 2000f, whatIsAGridLayer)
                ? hitInfo.transform.parent.GetComponent<GridCell>()
                : null;
        }

        private void SetChoice(int choice)
        {
            if (choice != (int)_choice)
            {
                _choice = (IconChoice)choice;
                EventSystem.current.SetSelectedGameObject(choicesPrefab[choice].iconButton.gameObject);
                return;
            }

            _lastSelected = null;
            EventSystem.current.SetSelectedGameObject(null);
            _choice = IconChoice.Default;
        }

        private void ButtonStaySelected()
        {
            if (EventSystem.current == null) return;
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                _lastSelected = EventSystem.current.currentSelectedGameObject;
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(_lastSelected);
            }
        }

        private void ManageSlideBloc(GridCell cellMouseIsOver)
        {
            var z = _terrainGrid.GetGridCellActualZ((Vector2Int)cellMouseIsOver.GridPosition);

            var topCell = _terrainGrid.GetCell(cellMouseIsOver.GridPosition.x, cellMouseIsOver.GridPosition.y, z);

            var prefab = topCell.Surface == GridCell.CellSurface.Flat
                ? choicesPrefab[(int)IconChoice.Slide].prefab
                : choicesPrefab[(int)IconChoice.Flat].prefab;

            _terrainGrid.ChangeCellZ((Vector2Int)cellMouseIsOver.GridPosition, prefab);
        }
    }
}