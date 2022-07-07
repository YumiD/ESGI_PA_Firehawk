using UnityEngine;

namespace FireCellScripts
{
    [RequireComponent(typeof(FireCell))]
    public class FireCellStarter : MonoBehaviour
    {
        private FireCell _fireCell;
        private bool _started;
        
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
            _started = !_started;
            _fireCell.DebugSetTemperature(_started ? 900f : 0f);
        }
    }
}
