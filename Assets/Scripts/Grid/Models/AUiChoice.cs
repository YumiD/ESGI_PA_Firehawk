using System;
using System.Collections.Generic;
using Grid.Interfaces;
using UI.Models;
using UnityEngine;

namespace Grid.Models
{
    public abstract class AUiChoice<T> : MonoBehaviour, IUiChoice
    {
        protected T Choice;

        public int GetChoice()
        {
            try
            {
                int enumChoice = Convert.ToInt32(Choice);
                return enumChoice;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }

        public virtual void SetChoice<T1>(int choice, IReadOnlyList<IconPrefab> choicesPrefab)
        {
            int currentChoice = choice;
            if (choicesPrefab[choice].icon.TryGetComponent(out AItemDropdown<T1> dropdown))
            {
                currentChoice = Convert.ToInt32(dropdown.ChoiceDropdown);
            }

            AIcon interactable = choicesPrefab[currentChoice].icon.GetComponent<AIcon>();
            interactable.SelectButton();
            try
            {
                if (currentChoice != Convert.ToInt32(Choice))
                {
                    for (int i = 0; i < choicesPrefab.Count; i++)
                    {
                        if (i != currentChoice)
                        {
                            choicesPrefab[i].icon.GetComponent<AIcon>().DeSelectButton();
                        }
                    }
                }
                else
                {
                    if (!choicesPrefab[currentChoice].icon.TryGetComponent(out AItemDropdown<T1> _))
                    {
                        interactable.DeSelectButton();
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