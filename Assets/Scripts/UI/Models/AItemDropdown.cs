using UnityEngine;
using UnityEngine.UI;

namespace UI.Models
{
    public abstract class AItemDropdown<T> : MonoBehaviour
    {
        private T _choiceDropdown; 
        public T ChoiceDropdown
        {
            get
            {
                ConvertToEnum();
                return _choiceDropdown;
            }
            protected set => _choiceDropdown = value;
        }
        protected Dropdown ItemDropdown;
        protected int CurrentIndex;

        private void Awake()
        {
            ItemDropdown = GetComponent<Dropdown>();
            ItemDropdown.onValueChanged.AddListener(delegate { GetValueDropdown(); });
        }

        public void GetValueDropdown()
        {
            CurrentIndex = ItemDropdown.value;
        }

        public abstract void ResetValue();

        protected abstract void ConvertToEnum();
    }
}
