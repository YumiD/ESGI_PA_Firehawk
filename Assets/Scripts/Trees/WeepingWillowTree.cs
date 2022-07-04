using System;
using FireCellScripts;
using Trees.Models;
using UnityEngine;

namespace Trees
{
    public class WeepingWillowTree : ATree
    {
        [SerializeField] private LayerMask grassMask;
        [SerializeField] private float radius = 4f;
        public override void OnBurn()
        {
            throw new NotImplementedException();
        }

        public override void OnFall()
        {
            var colliders = Physics.OverlapSphere(transform.position, radius);
            if (colliders.Length > 0)
            {
                foreach (var col in colliders)
                {
                    if (col.TryGetComponent(out FireCell cell))
                    {
                        cell.Extinct();
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}