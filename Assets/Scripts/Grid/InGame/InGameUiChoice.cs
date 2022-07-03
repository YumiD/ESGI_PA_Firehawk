using System;
using System.Collections.Generic;
using Grid.Interfaces;
using UI.Models;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Grid.InGame
{
    public class InGameUiChoice : MonoBehaviour, IUiChoice
    {
        private InGameIconChoice.IconChoice _choice;
        private GameObject _lastSelected;

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
            if (choice != (int)_choice)
            {
                _choice = (InGameIconChoice.IconChoice)choice;
                EventSystem.current.SetSelectedGameObject(choicesPrefab[choice].iconButton.gameObject);
                return;
            }

            _lastSelected = null;
            EventSystem.current.SetSelectedGameObject(null);
            _choice = InGameIconChoice.IconChoice.Default;
        }

        public void ButtonStaySelected()
        {
            if (EventSystem.current == null) return;
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                _lastSelected = EventSystem.current.currentSelectedGameObject;
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(_lastSelected);
            }
        }
    }
}