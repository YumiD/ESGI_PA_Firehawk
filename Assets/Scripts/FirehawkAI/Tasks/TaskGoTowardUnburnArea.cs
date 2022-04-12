using BehaviorTree;
using UnityEngine;

namespace FirehawkAI.Tasks
{
    public class TaskGoTowardUnburnArea : TaskPatrol
    {
        private Transform _pointToRunAwayFrom;
        private bool _choosePoint;

        public TaskGoTowardUnburnArea(Transform currentTransform, Transform[] waypoints, Vector3 groundTransform) :
            base(currentTransform, waypoints)
        {
        }

        public override NodeState Evaluate()
        {
            var colliders = Physics.OverlapSphere(CurrentTransform.position, FirehawkBT.DetectionRange,
                FirehawkBT.GroundLayerMask);
            if (colliders.Length > 0)
            {
                var count = 0;
                foreach (var col in colliders)
                {
                    if (!col.TryGetComponent<GridCell>(out var cell)) continue;
                    if (!cell.IsCurrentlyOnFire()) continue;
                    count++;
                }

                if (count <= colliders.Length * .05f)
                {
                    // Debug.Log($"Go toward only {count} cells on fire among {colliders.Length}");
                    // _pointToRunAwayFrom = cell.transform;
                    State = NodeState.SUCCESS;
                    return State;
                }   
            }

            State = NodeState.RUNNING;
            return State;
            // if (!_choosePoint)
            // {
            //     var unburnArea = FindPointToRunAwayFrom();
            //     if (unburnArea)
            //     {
            //         State = NodeState.SUCCESS;
            //         return State;
            //     }
            //
            //     State = NodeState.FAILURE;
            //     return State;
            // }
            //
            // // // Run away from point
            // // var direction = RunAway(CurrentTransform, _pointToRunAwayFrom).normalized;
            // // CurrentTransform.LookAt(direction);
            //
            // State = NodeState.RUNNING;
            // return State;
        }

        private static Vector3 RunAway(Transform a, Transform b)
        {
            return b.position - a.position;
        }

        private bool FindPointToRunAwayFrom()
        {
            var colliders = Physics.OverlapSphere(CurrentTransform.position, FirehawkBT.DetectionRange,
                FirehawkBT.GroundLayerMask);
            if (colliders.Length <= 0) return false;
            var count = 0;
            foreach (var col in colliders)
            {
                if (!col.TryGetComponent<GridCell>(out var cell)) continue;
                if (!cell.IsCurrentlyOnFire()) continue;
                count++;
            }

            if (count <= 5)
            {
                // _pointToRunAwayFrom = cell.transform;
                _choosePoint = true;
                return false;
            }

            return true;
        }
    }
}