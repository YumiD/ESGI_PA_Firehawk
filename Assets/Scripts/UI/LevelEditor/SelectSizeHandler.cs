using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.LevelEditor
{
    public class SelectSizeHandler : MonoBehaviour
    {
        [SerializeField]
        private LevelEditorUI levelEditorUI;
        private int _dropdownValue;
        void Start()
        {
            var dropdown = transform.GetComponent<Dropdown>();

            dropdown.options.Clear();

            List<string> items = new List<string>();
            items.Add("Small (10 X 10)");
            items.Add("Medium (20 X 20)");
            items.Add("Large (30 X 30)");

            foreach(var item in items){
                dropdown.options.Add(new Dropdown.OptionData(){text = item});
            }

            DropdownItemSelectedUpdate(dropdown);
            dropdown.onValueChanged.AddListener(delegate {DropdownItemSelectedUpdate(dropdown);});
        }

        void DropdownItemSelectedUpdate(Dropdown dropdown){
            _dropdownValue = dropdown.value;
        }

        public void GenerateLevel() {
            levelEditorUI.GenerateLevel(_dropdownValue);
        }
    }
}
