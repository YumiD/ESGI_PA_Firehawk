using System.Collections.Generic;
using Grid.Interfaces;
using UI.InGame;
using UI.Models;
using UnityEngine;

namespace Grid.InGame
{
    public class InGameUiChoice : MonoBehaviour, IUiChoice
    {
        private InGameIconChoice.IconChoice _choice;

        private void Start()
        {
            _choice = InGameIconChoice.IconChoice.Default;
        }

        public int GetChoice()
        {
            return (int)_choice;
        }

        public void SetChoice(int choice, IReadOnlyList<IconPrefab> choicesPrefab)
        {
            choicesPrefab[choice].icon.GetComponent<InGameUiIcon>().SelectButton();
            if (choice != (int)_choice)
            {
                _choice = (InGameIconChoice.IconChoice)choice;
                for (int i = 0; i < choicesPrefab.Count; i++)
                {
                    if (i != choice)
                    {
                        choicesPrefab[i].icon.GetComponent<InGameUiIcon>().DeSelectButton();
                    }
                }
            }
            else
            {
                choicesPrefab[choice].icon.GetComponent<InGameUiIcon>().DeSelectButton();
                _choice = InGameIconChoice.IconChoice.Default;
            }
        }
    }
}