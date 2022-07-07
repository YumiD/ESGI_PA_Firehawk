using Trees.Models;
using UnityEngine;

namespace Trees
{
    public class PalmTree : ATree
    {
        [SerializeField] private GameObject direction;

        public override void OnBurn()
        {
            throw new System.NotImplementedException();
        }

        public override void OnFall()
        {
            Rb = GetComponent<Rigidbody>();
            Rb.AddForce(-direction.transform.forward * 100f);
            direction.SetActive(false);
        }
    }
}
