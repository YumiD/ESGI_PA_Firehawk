using UI.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Models
{
    public abstract class AIcon : MonoBehaviour, IIcon
    {
        [SerializeField] protected Image btn;
        public abstract void UpdateQuantity(int qty);

        public virtual void SelectButton()
        {
            btn.color = Color.gray;
        }

        public virtual void DeSelectButton()
        {
            btn.color = Color.white;
        }
    }
}