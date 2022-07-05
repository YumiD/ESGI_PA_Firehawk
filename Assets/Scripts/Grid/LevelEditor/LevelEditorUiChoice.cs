using System.Collections.Generic;
using Grid.Interfaces;
using UI.LevelEditor;
using UI.Models;
using UnityEngine;

namespace Grid.LevelEditor
{
    public class LevelEditorUiChoice : MonoBehaviour, IUiChoice
    {
        private LevelEditorIconChoice.IconChoice _choice;

        private void Start()
        {
            _choice = LevelEditorIconChoice.IconChoice.Default;
        }

        public int GetChoice()
        {
            return (int)_choice;
        }

        public void SetChoice(int choice, IReadOnlyList<IconPrefab> choicesPrefab)
        {
            int currentChoice = choice;
            if (choicesPrefab[choice].icon.TryGetComponent(out LevelEditorDropdown dropdown))
            {
                currentChoice = (int)dropdown.ChoiceDropdown;
            }
            choicesPrefab[currentChoice].icon.GetComponent<LevelEditorUiIcon>().SelectButton();
            if (currentChoice != (int)_choice)
            {
                _choice = (LevelEditorIconChoice.IconChoice)currentChoice;
                for (int i = 0; i < choicesPrefab.Count; i++)
                {
                    if (i != currentChoice)
                    {
                        choicesPrefab[i].icon.GetComponent<LevelEditorUiIcon>().DeSelectButton();
                    }
                }
            }
            else
            {
                choicesPrefab[currentChoice].icon.GetComponent<LevelEditorUiIcon>().DeSelectButton();
                _choice = LevelEditorIconChoice.IconChoice.Default;
            }
        }
    }
}