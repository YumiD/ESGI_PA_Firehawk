using Events.Bool;
using UnityEngine;

namespace FireCellScripts
{
    public class FireCellEndGame : MonoBehaviour
    {
        [SerializeField] private EventBool stateGameEvent;
        [SerializeField] private float radius;
        [SerializeField] private LayerMask fireMask;
        private bool _winLose;

        private void Update()
        {
            if (_winLose)
            {
                return;
            }

            Collider[] colliders = new Collider[1];
            int size = Physics.OverlapSphereNonAlloc(transform.position, radius, colliders, fireMask);

            if (size < 1) return;
            
            _winLose = true;
            stateGameEvent.Raise(true);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
