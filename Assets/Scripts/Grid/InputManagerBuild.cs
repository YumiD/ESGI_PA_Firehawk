using System.Collections.Generic;
using Grid.Interfaces;
using UI.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Grid
{
    public class InputManagerBuild : MonoBehaviour
    {
        [SerializeField] private List<IconPrefab> choicesPrefab = new List<IconPrefab>();
        [SerializeField] private LayerMask whatIsAGridLayer;

        private IPlaceObject _putObject;
        private IUiChoice _uiChoice;

        private Camera _camera;
        private bool _canEdit = true;

        private void Start()
        {
            _camera = Camera.main;
            _putObject = GetComponent<IPlaceObject>();
            _uiChoice = GetComponent<IUiChoice>();
            AddButtonEvent();
        }

        private void AddButtonEvent()
        {
            for (int i = 0; i < choicesPrefab.Count; i++)
            {
                int numChoice = i;
                if (choicesPrefab[i].icon.TryGetComponent(out Button btn))
                {
                    btn.onClick.AddListener(delegate { _uiChoice.SetChoice(numChoice, choicesPrefab); });
                }
                if (choicesPrefab[i].icon.TryGetComponent(out Dropdown dropdown))
                {
                    dropdown.onValueChanged.AddListener(delegate { _uiChoice.SetChoice(numChoice, choicesPrefab); });
                }
            }
        }

        private void Update()
        {
            GridCell cellMouseIsOver = IsMouseOverAGridSpace();

            if (cellMouseIsOver == null) return;
            if (!_canEdit) return;

            _putObject.PutObject(cellMouseIsOver, _uiChoice.GetChoice(), choicesPrefab);

            KeyboardInput(cellMouseIsOver);
        }

        private void KeyboardInput(Component cellMouseIsOver)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _uiChoice.SetChoice(0, choicesPrefab);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _uiChoice.SetChoice(1, choicesPrefab);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                _uiChoice.SetChoice(2, choicesPrefab);
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                _uiChoice.SetChoice(3, choicesPrefab);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                cellMouseIsOver.transform.Rotate(0, 90, 0);
            }
        }

        // Returns grid cell if mouse is over it, else null
        private GridCell IsMouseOverAGridSpace()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            return Physics.Raycast(ray, out var hitInfo, 2000f, whatIsAGridLayer)
                ? hitInfo.transform.parent.GetComponent<GridCell>()
                : null;
        }

        public void ManageClick()
        {
            _canEdit = false;
            foreach (IconPrefab icon in choicesPrefab)
            {
                icon.icon.gameObject.GetComponent<Button>().interactable =
                    !icon.icon.gameObject.GetComponent<Button>().interactable;
            }
        }
    }
}