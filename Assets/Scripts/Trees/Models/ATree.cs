using FireCellScripts;
using UnityEngine;

namespace Trees.Models
{
    public abstract class ATree : MonoBehaviour
    {
        protected Rigidbody Rb;
        public abstract void OnBurn();
        public abstract void OnFall();
    }
}