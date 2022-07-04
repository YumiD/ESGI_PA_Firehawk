using UI.Models;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelEditorUiIcon : AIcon
    {
        private Image _btn;

        private void Start()
        {
            _btn = GetComponent<Image>();
        }

        public override void UpdateQuantity(int qty)
        {
            
        }

        public void SelectButton()
        {
            _btn.color = Color.gray;
        }
        public void DeSelectButton()
        {
            _btn.color = Color.white;
        }
    }
}