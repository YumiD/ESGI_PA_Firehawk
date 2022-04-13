using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManagerBuild : MonoBehaviour
{
	private TerrainGrid _terrainGrid;

	[SerializeField]
	private LayerMask whatIsAGridLayer;

	[SerializeField]
	private GameObject _tallGrassPrefab;

	[SerializeField]
	private GameObject _treePrefab;

	[SerializeField]
	private GameObject _flatTerrainPrefab;

	[SerializeField]
	private GameObject _slideTerrainPrefab;

	private void Start()
	{
		_terrainGrid = FindObjectOfType<TerrainGrid>();
	}

	private void Update()
	{
		GridCell cellMouseIsOver = IsMouseOverAGridSpace();
		if (cellMouseIsOver != null)
		{
			if (Input.GetMouseButtonDown(0))
			{
				Vector3Int gridPos = cellMouseIsOver.GridPosition;
				_terrainGrid.AddCellZ((Vector2Int)gridPos, _flatTerrainPrefab);
			}
			else if (Input.GetMouseButtonDown(1))
			{
				Vector3Int gridPos = cellMouseIsOver.GridPosition;
				_terrainGrid.RemoveCellZ((Vector2Int)gridPos);
			}
			else if (Input.GetMouseButtonDown(2))
			{
				int z = _terrainGrid.GetGridCellActualZ((Vector2Int)cellMouseIsOver.GridPosition);

				GridCell topCell = _terrainGrid.GetCell(cellMouseIsOver.GridPosition.x, cellMouseIsOver.GridPosition.y, z);
				
				GameObject prefab;
				if (topCell.Surface == GridCell.CellSurface.Flat)
				{
					prefab = _slideTerrainPrefab;
				}
				else
				{
					prefab = _flatTerrainPrefab;
				}
				
				_terrainGrid.ChangeCellZ((Vector2Int)cellMouseIsOver.GridPosition, prefab);
			}
			else if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				Vector3Int gridPos = cellMouseIsOver.GridPosition;
				_terrainGrid.SetObject(new Vector2Int(gridPos.x, gridPos.y), _treePrefab);
			}
			else if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				Vector3Int gridPos = cellMouseIsOver.GridPosition;
				_terrainGrid.SetObject(new Vector2Int(gridPos.x, gridPos.y), _tallGrassPrefab);
			}
			else if (Input.GetKeyDown(KeyCode.R))
			{
				cellMouseIsOver.transform.Rotate(0, 90, 0);
			}
		}
	}

	// Returns grid cell if mouse is over it, else null
	private GridCell IsMouseOverAGridSpace()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f, whatIsAGridLayer))
		{
			return hitInfo.transform.parent.GetComponent<GridCell>();
		}
		else
		{
			return null;
		}
	}
}