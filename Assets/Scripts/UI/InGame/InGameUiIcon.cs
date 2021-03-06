using TMPro;
using UI.Models;
using UnityEngine;

namespace UI.InGame
{
    public class InGameUiIcon : AIcon
    {
        [SerializeField] private TextMeshProUGUI quantityText;

        public override void UpdateQuantity(int qty)
        {
            quantityText.text = qty.ToString();
        }
    }
}
