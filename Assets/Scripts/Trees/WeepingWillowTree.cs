using FireCellScripts;
using Trees.Models;
using UnityEngine;

namespace Trees
{
    public class WeepingWillowTree : ATree
    {
        public override void OnBurn()
        {
            throw new System.NotImplementedException();
        }

        public override void OnFall(FireCell[] cells = default)
        {
            if (cells != null && cells.Length <= 0)
            {
                return;
            }
            
        }
    }
}