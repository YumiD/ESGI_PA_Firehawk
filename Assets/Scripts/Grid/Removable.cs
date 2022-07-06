using UnityEngine;

namespace Grid
{
    public class Removable : MonoBehaviour
    {
        [SerializeField] private bool isRemovable;

        public bool IsRemovable => isRemovable;
    }
}
