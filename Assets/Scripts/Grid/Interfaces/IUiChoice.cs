using System.Collections.Generic;
using UI.Models;

namespace Grid.Interfaces
{
    public interface IUiChoice
    {
        int GetChoice();
        void SetChoice<T1>(int choice, IReadOnlyList<IconPrefab> choicesPrefab);
    }
}