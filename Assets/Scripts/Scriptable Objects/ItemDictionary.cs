using System;
using UnityEngine;

namespace Scriptable_Objects
{
    [Serializable]
    public class ItemDictionary 
    {
        public FireObjectScriptableObject item;
        public int quantity;

        public ItemDictionary(FireObjectScriptableObject item, int quantity)
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