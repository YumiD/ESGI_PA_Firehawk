using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    GameGrid _gameGrid;
    [SerializeField] private LayerMask whatIsAGridLayer;


    void Start()
    {
        _gameGrid = FindObjectOfType<GameGrid>();
    }

    void Update()
    {
        GridCell cellMouseIsOver = IsMouseOverAGridSpace();
        if(cellMouseIsOver != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if(cellMouseIsOver.GetGridCellType() == EnumGridCell.GroundSlide)
                    _gameGrid.ChangeGridCell(cellMouseIsOver.GetPosition(), _gameGrid.GetGridCellActualZ(cellMouseIsOver.GetPosition()), EnumGridCell.GrassSlide);
                else
                    _gameGrid.ChangeGridCell(cellMouseIsOver.GetPosition(), _gameGrid.GetGridCellActualZ(cellMouseIsOver.GetPosition()), EnumGridCell.Grass);
            }
            if (Input.GetMouseButtonDown(1))
            {
                cellMouseIsOver.SetFire();
            }
            if(Input.GetMouseButtonDown(2)){
               _gameGrid.AddObject(cellMouseIsOver.GetPosition());
            }
            if(Input.GetKeyDown(KeyCode.Space)){
               _gameGrid.AddCellZ(cellMouseIsOver.GetPosition());
            }
            if(Input.GetKeyDown(KeyCode.A)){
                print(cellMouseIsOver.name);
                int x = int.Parse(cellMouseIsOver.name.Split('X')[1].Split(' ')[0]);
                int y = int.Parse(cellMouseIsOver.name.Split('Y')[1].Split(' ')[0]);
                int z = int.Parse(cellMouseIsOver.name.Split('Z')[1].Split(' ')[0]);
                print(x);
                print(y);
                print(z);
            }
            if(Input.GetKeyDown(KeyCode.E)){
                _gameGrid.ChangeGridCell(cellMouseIsOver.GetPosition(), _gameGrid.GetGridCellActualZ(cellMouseIsOver.GetPosition()), EnumGridCell.GroundSlide);
            }
            if(Input.GetKeyDown(KeyCode.R)){
                cellMouseIsOver.transform.Rotate(0,90,0);
            }
        }
    }

    // Returns grid cell if mouse is over it, else null
    private GridCell IsMouseOverAGridSpace()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hitInfo, 100f, whatIsAGridLayer))
        {
            return hitInfo.transform.GetComponent<GridCell>();
        }
        else
        {
            return null;
        }
    }
}
