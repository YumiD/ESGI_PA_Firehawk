using TMPro;
using UnityEngine;

namespace UI
{
    public class StateUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI stateText;
        
        public void OnChangedStateText(bool state)
        {
            stateText.text = state ? "You win" : "You lose";
            stateText.gameObject.SetActive(true);
        }
    }
}
