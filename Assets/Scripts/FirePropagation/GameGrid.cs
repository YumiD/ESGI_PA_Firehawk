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
        List<GridCell> neighborsList = new List<GridCell>();
        for(int i = -1; i<=1; i++){
            for(int j = -1; j<=1; j++){
                if(i==0 && j==0)
                    continue;
                if(posX+i<0 || posY+j<0 || posX+i>= width || posY+j >= height)
                    continue;
                if(gameGrid[posX+i, posY+j, 0].GetComponent<GridCell>()._canBeOnFire)
                    neighborsList.Add(gameGrid[posX+i, posY+j, 0].GetComponent<GridCell>());
            }
        }
        return neighborsList;
    }

    public void ChangeGridCell(Vector2Int pos, EnumGridCell gridCellType){
        int x = pos.x;
        int y = pos.y;
        Destroy(gameGrid[x, y, 0].GetComponent<GridCell>().gameObject);
        GameObject newGridCell = GetGameObjectFromEnum(gridCellType);
        gameGrid[x, y, 0] = Instantiate(newGridCell, new Vector3(x * GridSpaceSize, 0,  y * GridSpaceSize), Quaternion.identity);
        gameGrid[x, y, 0].transform.localScale = new Vector3(GridCellSize, GridCellSize/2, GridCellSize);
        gameGrid[x, y, 0].GetComponent<GridCell>().SetPosition(x, y);
        gameGrid[x, y, 0].transform.parent = transform;
        gameGrid[x, y, 0].gameObject.name = "Grid Space ( X: " + x.ToString() + " , Y: " + y.ToString() + " , Z: " + 0 + " )";
    }

    public void AddObject(Vector2Int pos){
        int x = pos.x;
        int y = pos.y;
        gameGrid[x, y, 0].GetComponent<GridCell>().SetObject(GO_Tree);
        gameGrid[x, y, 0].GetComponent<GridCell>().GetObject().name = "Tree ( X: " + x + " , Y: " + y + " , Z: " + 0 + " )";
        Instantiate(gameGrid[x, y, 0].GetComponent<GridCell>().GetObject(), new Vector3(x * GridSpaceSize, 0,  y * GridSpaceSize), Quaternion.identity);
    }

    public void AddCellZ(Vector2Int pos){
        int x = pos.x;
        int y = pos.y;
        int z = 0;
        while(z<maxZ){
            if(gameGrid[x, y, z] == null)
                break;
            z++;
        }
        print("z : " + z);
        gameGrid[x, y, z] = Instantiate(gridCellGround, new Vector3(x * GridSpaceSize, (z * GridSpaceSize /2),  y * GridSpaceSize), Quaternion.identity);
        gameGrid[x, y, z].transform.localScale = new Vector3(GridCellSize, GridCellSize/2, GridCellSize);
        gameGrid[x, y, z].GetComponent<GridCell>().SetPosition(x, y);
        gameGrid[x, y, z].transform.parent = transform;
        gameGrid[x, y, z].gameObject.name = "Grid Space ( X: " + x + " , Y: " + y + " , Z: "+ z + " )";
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
