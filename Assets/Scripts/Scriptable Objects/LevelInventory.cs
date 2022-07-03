using System.Collections.Generic;
using UnityEngine;

namespace Scriptable_Objects
{
    [CreateAssetMenu(fileName = "Inventory", menuName = "Level/Inventory")]
    public class LevelInventory : ScriptableObject
    {
        public List<ItemDictionary> inventory;
    }
}