using UnityEngine;

namespace FireCellScripts
{
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
