using UI.Interfaces;
using UnityEngine;

namespace UI.Models
{
    public abstract class AIcon : MonoBehaviour, IIcon
    {
        public abstract void UpdateQuantity(int qty);
    }
}