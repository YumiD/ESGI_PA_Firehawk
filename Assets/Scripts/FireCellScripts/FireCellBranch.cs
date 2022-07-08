using UnityEngine;

namespace FireCellScripts
{
    public class FireCellBranch : MonoBehaviour
    {
        [SerializeField] private Transform root;
        public void BurnBranch()
        {
            root.gameObject.layer = 8; //litBranch
        }

        public void FinishSmoking()
        {
            transform.root.gameObject.layer = 9; //UsedLitBranch
        }
    }
}
