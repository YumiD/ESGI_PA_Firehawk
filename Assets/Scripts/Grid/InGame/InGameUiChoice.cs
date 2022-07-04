using System.Collections.Generic;
using Grid.Interfaces;
using UI;
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

        public void SetChoice(int choice, IReadOnlyList<ButtonPrefab> choicesPrefab)
        {
            choicesPrefab[choice].iconButton.GetComponent<LevelEditorUiIcon>().SelectButton();
            if (choice != (int)_choice)
            {
                _choice = (InGameIconChoice.IconChoice)choice;
                for (int i = 0; i < choicesPrefab.Count; i++)
                {
                    if (i != choice)
                    {
                        choicesPrefab[i].iconButton.GetComponent<LevelEditorUiIcon>().DeSelectButton();
                    }
                }
            }
            else
            {
                choicesPrefab[choice].iconButton.GetComponent<LevelEditorUiIcon>().DeSelectButton();
                _choice = InGameIconChoice.IconChoice.Default;
            }
        }
    }
}