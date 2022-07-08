using System.Collections.Generic;
using FireCellScripts;
using UnityEngine;

public class FireManager : MonoBehaviour
{
    public static FireManager Instance { get; private set; }
    
    private List<FireCell> _cells = new List<FireCell>();
    
    private List<FireCell> _burningCells = new List<FireCell>();
    private List<FireCell> _cellsThatCanBurn = new List<FireCell>();
	
    public void RegisterFireCell(FireCell cell)
    {
        _cells.Add(cell);

        _burningCells.Capacity = _cells.Capacity;
        _cellsThatCanBurn.Capacity = _cells.Capacity;
    }
	
    public void UnregisterFireCell(FireCell cell)
    {
        _cells.Remove(cell);
    }

    private void Awake()
    {
        Instance = this;
    }

    private void FixedUpdate()
    {
        _burningCells.Clear();
        _cellsThatCanBurn.Clear();
        
        for (int i = 0; i < _cells.Count; i++)
        {
            if (_cells[i].FireState == FireState.OnFire)
            {
                _burningCells.Add(_cells[i]);
            }
            else if (_cells[i].Fuel > 0)
            {
                _cellsThatCanBurn.Add(_cells[i]);
            }
        }

        int callsAfterOpt = 0;
        for (int i = 0; i < _burningCells.Count; i++)
        {
            for (int j = 0; j < _cellsThatCanBurn.Count; j++)
            {
                float distanceCenterToCenter = Vector3.Distance(_burningCells[i].transform.position, _cellsThatCanBurn[j].transform.position);
                float distanceEdgeToEdge = distanceCenterToCenter - _burningCells[i].Radius - _cellsThatCanBurn[j].Radius;

                if (_burningCells[i].ShouldBeExtinct)
                {                    
                    _burningCells[i].OnPutOutFire(_cellsThatCanBurn[j], distanceEdgeToEdge);
                }
                else
                {
                    _burningCells[i].OnPropagate(_cellsThatCanBurn[j], distanceCenterToCenter, distanceEdgeToEdge);
                }

                callsAfterOpt++;
            }
        }
    }
}
