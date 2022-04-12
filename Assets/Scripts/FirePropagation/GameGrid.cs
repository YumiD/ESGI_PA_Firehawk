using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{

    public int height = 30;
    private int width = 30;
    private int maxZ = 10;
    private float GridCellSize = 4f;

    [SerializeField] private bool generation = true;

    [SerializeField] private GameObject gridCellGround;
    [SerializeField] private GameObject gridCellGroundSlide;
    [SerializeField] private GameObject gridCellGrass;
    [SerializeField] private GameObject gridCellGrassSlide;
    [SerializeField] private GameObject gridCellBurned;

    [SerializeField] private GameObject GO_Tree;

    private GameObject[,,] _gameGrid;

    void Start()
    {
        if(generation)
            CreateGrid();
        else
            BuildGrid();
    }

    private void CreateGrid()
    {
        _gameGrid = new GameObject[height, width, maxZ];

        if (gridCellGround == null || gridCellGroundSlide == null || gridCellGrassSlide == null || gridCellGrass == null || gridCellBurned == null)
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
                CreateGridCell(x, y, 0, EnumGridCell.Ground);
            }
        }
    }

    private void CreateGridCell(int x, int y, int z, EnumGridCell gridCellType){
        GameObject newGridCell = GetGameObjectFromEnum(gridCellType);
        _gameGrid[x, y, z] = Instantiate(newGridCell, new Vector3(x * GridCellSize, z * (GridCellSize /2),  y * GridCellSize), Quaternion.identity);
        _gameGrid[x, y, z].transform.localScale = new Vector3(GridCellSize, GridCellSize/2, GridCellSize);
        _gameGrid[x, y, z].GetComponent<GridCell>().SetPosition(x, y);
        _gameGrid[x, y, z].GetComponent<GridCell>().SetGridCellType(gridCellType);
        _gameGrid[x, y, z].transform.parent = transform;
        _gameGrid[x, y, z].gameObject.name = "Grid Space ( X" + x + " , Y" + y + " , Z"+ z + " )";

        // TODO Refaire les prefabs pour éviter d'avoir ces lignes de code
        if(gridCellType == EnumGridCell.GroundSlide || gridCellType == EnumGridCell.GrassSlide)
            _gameGrid[x, y, z].transform.localScale = new Vector3(GridCellSize/2, GridCellSize/4, GridCellSize/2);
    }

    // If the grid was always generated, we fill the array _gameGrid with existent gridCells
    public void BuildGrid(){
        _gameGrid = new GameObject[height, width, maxZ];

        if (gridCellGround == null || gridCellGroundSlide == null || gridCellGrassSlide == null || gridCellGrass == null || gridCellBurned == null)
        {
            Debug.Log("ERROR: Not Assigned");
            return;
        }

        foreach(Transform child in transform)
        {
            int x = int.Parse(child.gameObject.name.Split('X')[1].Split(' ')[0]);
            int y = int.Parse(child.gameObject.name.Split('Y')[1].Split(' ')[0]);
            int z = int.Parse(child.gameObject.name.Split('Z')[1].Split(' ')[0]);
            _gameGrid[x, y, z] = child.gameObject;
            _gameGrid[x, y, z].GetComponent<GridCell>().SetPosition(x, y);
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
                if(_gameGrid[posX+i, posY+j, z]==null)
                    continue;
                if(_gameGrid[posX+i, posY+j, z].GetComponent<GridCell>()._canBeOnFire)
                    neighborsList.Add(_gameGrid[posX+i, posY+j, z].GetComponent<GridCell>());
            }
        }
        return neighborsList;
    }

    public void ChangeGridCell(Vector2Int posXY, int posZ, EnumGridCell gridCellType){
        int x = posXY.x;
        int y = posXY.y;
        int z = posZ;
        Destroy(_gameGrid[x, y, z].GetComponent<GridCell>().gameObject);
        CreateGridCell(x, y, z, gridCellType);
        if(gridCellType == EnumGridCell.Grass){
            _gameGrid[x, y, z].GetComponent<GridCell>()._canBeOnFire = true;
            _gameGrid[x, y, z].GetComponent<GridCell>()._isOnFire = false;
        }
    }

    public void AddObject(Vector2Int pos){
        int x = pos.x;
        int y = pos.y;
        int z = GetGridCellActualZ(pos);

        if(_gameGrid[x, y, z].GetComponent<GridCell>().GetObject()!=null)
            _gameGrid[x, y, z].GetComponent<GridCell>().RemoveObject();

        _gameGrid[x, y, z].GetComponent<GridCell>().SetObjectReference(GO_Tree);
        _gameGrid[x, y, z].GetComponent<GridCell>()._isOccupied = true;
        _gameGrid[x, y, z].GetComponent<GridCell>()._gameObject = Instantiate(_gameGrid[x, y, z].GetComponent<GridCell>().GetObjectReference(), new Vector3(x * GridCellSize, z * (GridCellSize /2),  y * GridCellSize), Quaternion.identity);
        _gameGrid[x, y, z].GetComponent<GridCell>().GetObject().name = "GameObject ( X" + x + " , Y" + y + " , Z" + z + " )";
        _gameGrid[x, y, z].GetComponent<GridCell>().GetObject().transform.parent = _gameGrid[x, y, z].GetComponent<GridCell>().transform;
    }

    public void AddCellZ(Vector2Int pos){
        int x = pos.x;
        int y = pos.y;
        int z = GetGridCellActualZ(pos) + 1;
        if(z>=maxZ)
            return;
        if(_gameGrid[x, y, z-1].GetComponent<GridCell>().GetObject()!=null)
            _gameGrid[x, y, z-1].GetComponent<GridCell>().RemoveObject();
        ChangeGridCell(pos, z-1, EnumGridCell.Ground);
        CreateGridCell(x, y, z, EnumGridCell.Ground);
    }

    public int GetGridCellActualZ(Vector2Int pos){
        int x = pos.x;
        int y = pos.y;
        int z = 0;
        while(z<maxZ){
            if(_gameGrid[x, y, z] == null)
                break;
            z++;
        }
        return z-1;
    }

    public Vector2Int GetGridPosFromWorld(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt(worldPosition.x / GridCellSize);
        int y = Mathf.FloorToInt(worldPosition.z / GridCellSize);

        x = Mathf.Clamp(x, 0, width);
        y = Mathf.Clamp(y, 0, height);

        return new Vector2Int(x, y);
    }

    public Vector3 GetWorldPosFromGridPos(Vector3 gridPos)
    {
        float x = gridPos.x * GridCellSize;
        float y = gridPos.y * GridCellSize;

        return new Vector3(x, 0, y);
    }

    public GameObject GetGameObjectFromEnum(EnumGridCell gridCellType){
        switch(gridCellType){
            case EnumGridCell.Ground:
                return gridCellGround;
            case EnumGridCell.GroundSlide:
                return gridCellGroundSlide;
            case EnumGridCell.Grass:
                return gridCellGrass;
            case EnumGridCell.GrassSlide:
                return gridCellGrassSlide;
            case EnumGridCell.Burned:
                return gridCellBurned;
            default:
                return gridCellGround;
        }
    }
}
