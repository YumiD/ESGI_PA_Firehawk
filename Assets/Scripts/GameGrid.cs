using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{

    private int height = 10;
    private int width = 10;
    private float GridSpaceSize = 5.1f;

    [SerializeField] private GameObject gridCellPrefab;
    private GameObject[,] gameGrid;

    void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        gameGrid = new GameObject[height, width];

        if (gridCellPrefab == null)
        {
            Debug.Log("ERROR: Not Assigned");
            return;
        }

        //Create Grid
        for(int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                //Create GridSpace object for each cell
                gameGrid[x, y] = Instantiate(gridCellPrefab, new Vector3(x * GridSpaceSize, 0,  y * GridSpaceSize), Quaternion.identity);
                //gameGrid[x, y].transform.Rotate(90,0,0);
                gameGrid[x, y].GetComponent<GridCell>().SetPosition(x, y);
                gameGrid[x, y].transform.parent = transform;
                gameGrid[x, y].gameObject.name = "Grid Space ( X: " + x.ToString() + " , Y: " + y.ToString() + ")";
            }
        }
    }

    public Vector2Int GetGridPosFromWorld(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt(worldPosition.x / GridSpaceSize);
        int y = Mathf.FloorToInt(worldPosition.z / GridSpaceSize);

        x = Mathf.Clamp(x, 0, width);
        y = Mathf.Clamp(y, 0, height);

        return new Vector2Int(x, y);
    }

    public Vector3 GetWorldPosFromGridPos(Vector3 gridPos)
    {
        float x = gridPos.x * GridSpaceSize;
        float y = gridPos.y * GridSpaceSize;

        return new Vector3(x, 0, y);
    }
}
