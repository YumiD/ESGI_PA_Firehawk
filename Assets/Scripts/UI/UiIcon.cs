using TMPro;
using UnityEngine;

namespace UI
{
    public class UiIcon : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI quantityText;

        public void UpdateQuantity(int qty)
        {
            quantityText.text = qty.ToString();
        }
    }
}
