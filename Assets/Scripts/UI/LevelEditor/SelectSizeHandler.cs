using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectSizeHandler : MonoBehaviour
{
    [SerializeField]
    private LevelEditorUI levelEditorUI;
    private int dropdownValue = 0;
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
        dropdownValue = dropdown.value;
    }

    public void GenerateLevel() {
        levelEditorUI.GenerateLevel(dropdownValue);
    }
}
