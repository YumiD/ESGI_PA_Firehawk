using System.Collections.Generic;
using UnityEngine;

public class FireManager : MonoBehaviour
{
    public static FireManager Instance { get; private set; }
    
    private List<FireCell> _cells = new List<FireCell>();
	
    public void RegisterFireCell(FireCell cell)
    {
        _cells.Add(cell);
    }

    private void Awake()
    {
        Instance = this;
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < _cells.Count; i++)
        {
            for (int j = i+1; j < _cells.Count; j++)
            {
                float distanceCenterToCenter = Vector3.Distance(_cells[i].transform.position, _cells[j].transform.position);
                float distanceEdgeToEdge = distanceCenterToCenter - _cells[i].Radius - _cells[j].Radius;
				
                _cells[i].OnPropagate(_cells[j], distanceCenterToCenter, distanceEdgeToEdge);
                _cells[j].OnPropagate(_cells[i], distanceCenterToCenter, distanceEdgeToEdge);
            }
        }
    }
}
