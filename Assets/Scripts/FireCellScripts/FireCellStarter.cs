using UnityEngine;

namespace FireCellScripts
{
    [RequireComponent(typeof(FireCell))]
    public class FireCellStarter : MonoBehaviour
    {
        private FireCell _fireCell;
        
        private void Start()
        {
            _fireCell = GetComponent<FireCell>();
        }

        public void StartFire()
        {
            _fireCell.DebugSetTemperature(900f);
        }
    }
}
