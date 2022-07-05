using UnityEngine;
using UnityEngine.UI;

namespace UI.Models
{
    public abstract class AItemDropdown : MonoBehaviour
    {
        private Dropdown _itemDropdown;
        protected int CurrentIndex;

        private void Awake()
        {
            _itemDropdown = GetComponent<Dropdown>();
            _itemDropdown.onValueChanged.AddListener(delegate { GetValueDropdown(); });
        }

        public void GetValueDropdown()
        {
            CurrentIndex = _itemDropdown.value;
        }

        public abstract void ConvertToEnum();
    }
}
