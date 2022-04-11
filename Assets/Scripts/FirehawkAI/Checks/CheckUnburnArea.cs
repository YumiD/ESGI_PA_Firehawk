using BehaviorTree;
using UnityEngine;

namespace FirehawkAI.Checks
{
    public class CheckUnburnArea : Node
    {
        private readonly Transform _transform;

        public CheckUnburnArea(Transform transform)
        {
            _transform = transform;
        }

        public override NodeState Evaluate()
        {
            var colliders = Physics.OverlapSphere(_transform.position, FirehawkBT.DetectionRange,
                FirehawkBT.GroundLayerMask);
            if (colliders.Length > 0)
            {
                Debug.Log("Looking for unburn area");
                var count = 0;
                foreach (var col in colliders)
                {
                    if (!col.TryGetComponent<GridCell>(out var cell)) continue;
                    if (!cell.IsCurrentlyOnFire()) continue;
                    count++;
                }

                if (count > colliders.Length * .1f)
                {
                    // Debug.Log($"Check only {count} cells on fire among {colliders.Length}");
                    State = NodeState.SUCCESS;
                    return State;
                }
            }

            State = NodeState.FAILURE;
            return State;
        }
    }
}