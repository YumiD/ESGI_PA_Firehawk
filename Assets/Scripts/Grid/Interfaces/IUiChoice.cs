using System.Collections.Generic;
using UI.Models;

namespace Grid.Interfaces
{
    public interface IUiChoice
    {
        int GetChoice();
        void SetChoice(int choice, IReadOnlyList<IconPrefab> choicesPrefab);
    }
}