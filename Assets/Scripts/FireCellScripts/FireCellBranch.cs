using FirehawkAI;
using UnityEngine;

namespace FireCellScripts
{
    public class FireCellBranch : MonoBehaviour
    {
        public void BurnBranch()
        {
            transform.root.gameObject.layer = 8; //litBranch
        }

        public void FinishSmoking()
        {
            transform.root.gameObject.layer = 9; //UsedLitBranch
        }
    }
}
