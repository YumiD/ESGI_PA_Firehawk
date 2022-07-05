using Trees.Models;
using UnityEngine;

namespace Trees
{
    public class BombTree : ATree
    {
        [SerializeField] private Seed seed;
        public override void OnBurn()
        {
            throw new System.NotImplementedException();
        }

        public override void OnFall()
        {
            seed.gameObject.SetActive(true);
        }
    }
}
