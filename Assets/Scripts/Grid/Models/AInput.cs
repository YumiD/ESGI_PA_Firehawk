using System;
using System.Collections.Generic;
using Grid.Interfaces;
using UI.Models;
using UnityEngine;

namespace Grid.Models
{
    public abstract class AInput : MonoBehaviour, IPlaceObject
    {
        protected TerrainGrid TerrainGrid;

        private void Start()
        {
            TerrainGrid = FindObjectOfType<TerrainGrid>();
        }

        public abstract void PutObject(GridCell cellMouseIsOver, int choice, List<ButtonPrefab> choicesPrefab);
    }
}