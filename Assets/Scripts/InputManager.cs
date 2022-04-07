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
        if(cellMouseIsOver != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                print("HEY");
                cellMouseIsOver.GetComponentInChildren<MeshRenderer>().material.color = Color.green;
            }
        }
    }

    // Returns grid cell if mouse is over it, else null
    private GridCell IsMouseOverAGridSpace()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hitInfo, 100f, whatIsAGridLayer))
        {
            print(hitInfo.transform.GetComponent<GridCell>());
            return hitInfo.transform.GetComponent<GridCell>();
        }
        else
        {
            return null;
        }
    }
}
