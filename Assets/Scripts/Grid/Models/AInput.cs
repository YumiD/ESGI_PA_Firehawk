using System;
using System.Collections.Generic;
using Grid.Interfaces;
using UI.Models;
using UnityEngine;

namespace Grid.Models
{
    public abstract class AInput : MonoBehaviour, IPlaceObject
    {
        private TerrainGrid _terrainGrid;
        protected TerrainGrid TerrainGrid
        {
            get => FindTerrainGrid();
            set => _terrainGrid = value;
        }

        private void OnEnable()
        {
            FindTerrainGrid();
        }

        private TerrainGrid FindTerrainGrid()
        {
            if (_terrainGrid == null)
            {
                _terrainGrid = FindObjectOfType<TerrainGrid>();
            }

            return _terrainGrid;
        }

        public abstract void PutObject(GridCell cellMouseIsOver, int choice, List<IconPrefab> choicesPrefab);
    }
}