using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{

    private int height = 30;
    private int width = 30;
    private int maxZ = 10;
    private float GridCellSize = 2f;
    private float GridSpaceSize = 2f;

    [SerializeField] private GameObject gridCellGround;
    [SerializeField] private GameObject gridCellGrass;
    [SerializeField] private GameObject gridCellBurned;

    [SerializeField] private GameObject GO_Tree;

    private GameObject[,,] gameGrid;

    void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        gameGrid = new GameObject[height, width, maxZ];

        if (gridCellGround == null)
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
                gameGrid[x, y, 0] = Instantiate(gridCellGround, new Vector3(x * GridSpaceSize, 0,  y * GridSpaceSize), Quaternion.identity);
                gameGrid[x, y, 0].transform.localScale = new Vector3(GridCellSize, GridCellSize/2, GridCellSize);
                gameGrid[x, y, 0].GetComponent<GridCell>().SetPosition(x, y);
                gameGrid[x, y, 0].transform.parent = transform;
                gameGrid[x, y, 0].gameObject.name = "Grid Space ( X: " + x + " , Y: " + y + " , Z: "+ 0 + " )";
            }
        }
    }

    // Get Neighbors that can be on fire from a cell position
    public List<GridCell> GetFlammableNeighbors(int posX, int posY){
        int z = GetGridCellActualZ(new Vector2Int(posX, posY));
        List<GridCell> neighborsList = new List<GridCell>();
        for(int i = -1; i<=1; i++){
            for(int j = -1; j<=1; j++){
                if(i==0 && j==0)
                    continue;
                if(posX+i<0 || posY+j<0 || posX+i>= width || posY+j >= height)
                    continue;
                if(gameGrid[posX+i, posY+j, z]==null)
                    continue;
                if(gameGrid[posX+i, posY+j, z].GetComponent<GridCell>()._canBeOnFire)
                    neighborsList.Add(gameGrid[posX+i, posY+j, z].GetComponent<GridCell>());
            }
        }
        return neighborsList;
    }

    public void ChangeGridCell(Vector2Int pos, EnumGridCell gridCellType){
        int x = pos.x;
        int y = pos.y;
        int z = GetGridCellActualZ(pos);
        //int z = 0;
        Destroy(gameGrid[x, y, z].GetComponent<GridCell>().gameObject);
        GameObject newGridCell = GetGameObjectFromEnum(gridCellType);
        gameGrid[x, y, z] = Instantiate(newGridCell, new Vector3(x * GridSpaceSize, (z * GridSpaceSize)/2,  y * GridSpaceSize), Quaternion.identity);
        gameGrid[x, y, z].transform.localScale = new Vector3(GridCellSize, GridCellSize/2, GridCellSize);
        gameGrid[x, y, z].GetComponent<GridCell>().SetPosition(x, y);
        gameGrid[x, y, z].transform.parent = transform;
        gameGrid[x, y, z].gameObject.name = "Grid Space ( X: " + x.ToString() + " , Y: " + y.ToString() + " , Z: " + z.ToString() + " )";
        if(gridCellType == EnumGridCell.Grass){
            gameGrid[x, y, z].GetComponent<GridCell>()._canBeOnFire = true;
            gameGrid[x, y, z].GetComponent<GridCell>()._isOnFire = false;
        }
    }

    public void AddObject(Vector2Int pos){
        int x = pos.x;
        int y = pos.y;
        int z = GetGridCellActualZ(pos);
        // TODO Corriger ce Z = 0
        gameGrid[x, y, 0].GetComponent<GridCell>().SetObjectReference(GO_Tree);
        gameGrid[x, y, 0].GetComponent<GridCell>()._isOccupied = true;
        gameGrid[x, y, 0].GetComponent<GridCell>()._gameObject = Instantiate(gameGrid[x, y, 0].GetComponent<GridCell>().GetObjectReference(), new Vector3(x * GridSpaceSize, (z * GridSpaceSize /2),  y * GridSpaceSize), Quaternion.identity);
        gameGrid[x, y, 0].GetComponent<GridCell>().GetObject().name = "GameObject ( X: " + x + " , Y: " + y + " , Z: " + z + " )";
        gameGrid[x, y, 0].GetComponent<GridCell>().GetObject().transform.parent = gameGrid[x, y, 0].GetComponent<GridCell>().transform;
    }

    public void AddCellZ(Vector2Int pos){
        int x = pos.x;
        int y = pos.y;
        int z = GetGridCellActualZ(pos) + 1;
        if(z>=maxZ)
            return;
        // TODO Remplacer le 0 par (z-1) quand le todo de AddObject sera completé
        if(gameGrid[x, y, 0].GetComponent<GridCell>().GetObject()!=null){
            gameGrid[x, y, 0].GetComponent<GridCell>().RemoveObject();
        }
        gameGrid[x, y, z] = Instantiate(gridCellGround, new Vector3(x * GridSpaceSize, (z * GridSpaceSize /2),  y * GridSpaceSize), Quaternion.identity);
        gameGrid[x, y, z].transform.localScale = new Vector3(GridCellSize, GridCellSize/2, GridCellSize);
        gameGrid[x, y, z].GetComponent<GridCell>().SetPosition(x, y);
        gameGrid[x, y, z].transform.parent = transform;
        gameGrid[x, y, z].gameObject.name = "Grid Space ( X: " + x + " , Y: " + y + " , Z: "+ z + " )";
    }

    public int GetGridCellActualZ(Vector2Int pos){
        int x = pos.x;
        int y = pos.y;
        int z = 0;
        while(z<maxZ){
            if(gameGrid[x, y, z] == null)
                break;
            z++;
        }
        return z-1;
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

    public GameObject GetGameObjectFromEnum(EnumGridCell gridCellType){
        switch(gridCellType){
            case EnumGridCell.Ground:
                return gridCellGround;
            case EnumGridCell.Grass:
                return gridCellGrass;
            case EnumGridCell.Burned:
                return gridCellBurned;
            default:
                return gridCellGround;
        }
    }
}
