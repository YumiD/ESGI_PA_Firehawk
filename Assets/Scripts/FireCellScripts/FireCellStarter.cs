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
            if (_fireCell == null)
            {
                _fireCell = GetComponent<FireCell>();
            }
            _fireCell.DebugSetTemperature(900f);
        }
    }
}
