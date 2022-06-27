using Trees.Interfaces;
using UnityEngine;

namespace Trees
{
    public class PalmTree : MonoBehaviour, ITree
    {
        [SerializeField] private GameObject direction;
        private Rigidbody _rb;

        public void Burn()
        {
            throw new System.NotImplementedException();
        }

        public void Fall()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.AddForce(-direction.transform.forward * 100f);
            direction.SetActive(false);
        }
    }
}
