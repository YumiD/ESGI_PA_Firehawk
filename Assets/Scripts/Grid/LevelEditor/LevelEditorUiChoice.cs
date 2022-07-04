using System.Collections.Generic;
using Grid.Interfaces;
using UI;
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

        public void SetChoice(int choice, IReadOnlyList<ButtonPrefab> choicesPrefab)
        {
            choicesPrefab[choice].iconButton.GetComponent<LevelEditorUiIcon>().SelectButton();
            if (choice != (int)_choice)
            {
                _choice = (LevelEditorIconChoice.IconChoice)choice;
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
                _choice = LevelEditorIconChoice.IconChoice.Default;
            }
        }
    }
}