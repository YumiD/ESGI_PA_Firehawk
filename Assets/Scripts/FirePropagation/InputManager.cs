using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    GameGrid gameGrid;
    [SerializeField] private LayerMask whatIsAGridLayer;


    void Start()
    {
        gameGrid = FindObjectOfType<GameGrid>();
    }

    void Update()
    {
        GridCell cellMouseIsOver = IsMouseOverAGridSpace();
        EnumGridCell gridCellType = EnumGridCell.Grass;
        if(cellMouseIsOver != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                cellMouseIsOver._canBeOnFire = true;
                cellMouseIsOver._isOnFire = false;
                gameGrid.ChangeGridCell(cellMouseIsOver.GetPosition(), gridCellType);
            }
            if (Input.GetMouseButtonDown(1))
            {
                cellMouseIsOver.SetFire();
            }
            if(Input.GetMouseButtonDown(2)){
               gameGrid.AddObject(cellMouseIsOver.GetPosition());
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
