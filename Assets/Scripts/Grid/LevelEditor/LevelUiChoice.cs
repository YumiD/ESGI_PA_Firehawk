using System;
using System.Collections.Generic;
using Grid.Models;
using UI.Models;
using UnityEngine;

namespace Grid.LevelEditor
{
    public class LevelUiChoice : AUiChoice<AIconChoice.IconChoice>
    {
        private void Start()
        {
            Choice = AIconChoice.IconChoice.Default;
        }

        public override void SetChoice<T>(int choice, IReadOnlyList<IconPrefab> choicesPrefab)
        {
            base.SetChoice<T>(choice, choicesPrefab);
            int currentChoice = choice;
            try
            {
                if (choicesPrefab[choice].icon.TryGetComponent(out AItemDropdown<T> dropdown))
                {
                    currentChoice = Convert.ToInt32(dropdown.ChoiceDropdown);
                }
                if (currentChoice != Convert.ToInt32(Choice))
                {
                    Choice = (AIconChoice.IconChoice)currentChoice;
                }
                else
                {
                    if (!choicesPrefab[currentChoice].icon.TryGetComponent(out AItemDropdown<T> _))
                    {
                        Choice = AIconChoice.IconChoice.Default;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }
    }
}