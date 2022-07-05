using System.Collections.Generic;
using Grid.Interfaces;
using UI.Models;
using UnityEngine;

namespace Grid.Models
{
    public abstract class AInput : MonoBehaviour, IPlaceObject
    {
        [SerializeField]protected TerrainGrid terrainGrid;

        private void OnEnable()
        {
            if (terrainGrid == null)
            {
                terrainGrid = FindObjectOfType<TerrainGrid>();
            }
        }

        public abstract void PutObject(GridCell cellMouseIsOver, int choice, List<IconPrefab> choicesPrefab);
    }
}