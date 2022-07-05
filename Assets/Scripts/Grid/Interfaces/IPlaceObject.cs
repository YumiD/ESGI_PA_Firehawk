using System.Collections.Generic;
using UI.Models;

namespace Grid.Interfaces
{
    public interface IPlaceObject
    {
        void PutObject(GridCell cellMouseIsOver, int choice, List<IconPrefab> choicesPrefab);
    }
}