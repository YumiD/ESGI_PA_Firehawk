using System;
using UnityEngine;

namespace Scriptable_Objects
{
    [Serializable]
    public class ItemDictionary 
    {
        public GameObject item;
        public int quantity;

        public ItemDictionary(GameObject item, int quantity)
        {
            this.item = item;
            this.quantity = quantity;
        }

        public void AddItem()
        {
            quantity++;
        }
        
        public void RemoveItem()
        {
            quantity--;
        }

        public bool VerifyItem()
        {
            return quantity > 0;
        }
    }
}