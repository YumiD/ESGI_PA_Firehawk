using Trees.Models;
using UnityEngine;

namespace Trees
{
    public class BombTree : ATree
    {
        [SerializeField] private Seed seed;
        [SerializeField] private GameObject direction;
        public override void OnBurn()
        {
            throw new System.NotImplementedException();
        }

        public override void OnFall()
        {
            Rb = GetComponent<Rigidbody>();
            Rb.AddForce(-direction.transform.forward * -100f);
            seed.gameObject.SetActive(true);
            direction.SetActive(false);
        }
    }
}
