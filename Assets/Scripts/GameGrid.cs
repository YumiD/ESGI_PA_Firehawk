using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{

    private int height = 30;
    private int width = 30;
    private float GridCellSize = 2f;
    private float GridSpaceSize = 2f;

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
                gameGrid[x, y].transform.localScale = new Vector3(GridCellSize, GridCellSize/2, GridCellSize);
                gameGrid[x, y].GetComponent<GridCell>().SetPosition(x, y);
                gameGrid[x, y].transform.parent = transform;
                gameGrid[x, y].gameObject.name = "Grid Space ( X: " + x.ToString() + " , Y: " + y.ToString() + ")";
            }
        }
    }

    // Get Neighbors that can be on fire from a cell position
    public List<GridCell> GetFlammableNeighbors(int posX, int posY){
        List<GridCell> neighborsList = new List<GridCell>();
        for(int i = -1; i<=1; i++){
            for(int j = -1; j<=1; j++){
                if(i==0 && j==0)
                    continue;
                if(posX+i<0 || posY+j<0 || posX+i>= width || posY+j >= height)
                    continue;
                if(gameGrid[posX+i, posY+j].GetComponent<GridCell>()._canBeOnFire)
                    neighborsList.Add(gameGrid[posX+i, posY+j].GetComponent<GridCell>());
            }
        }
        return neighborsList;
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
