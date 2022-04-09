using BehaviorTree;
using UnityEngine;

namespace FirehawkAI.Tasks
{
    public class TaskGoTowardUnburnArea : Node
    {
        private Transform _transform;
        private Transform _pointToRunAwayFrom;
        private bool _choosePoint;
        public TaskGoTowardUnburnArea(Transform transform)
        {
            _transform = transform;
        }
        public override NodeState Evaluate()
        {
            if (!_choosePoint)
            {
                FindPointToRunAwayFrom();
            }
            else
            {
                // Run away from point
                var direction = RunAway(_transform, _pointToRunAwayFrom).normalized;
                _transform.LookAt(direction);
            }

            State = NodeState.SUCCESS;
            return State;
        }

        private static Vector3 RunAway(Transform a, Transform b)
        {
            return b.position - a.position;
        }

        private void FindPointToRunAwayFrom()
        {
            var colliders = Physics.OverlapSphere(_transform.position, FirehawkBT.DetectionRange,
                FirehawkBT.GroundLayerMask);
            if (colliders.Length <= 0) return;
            foreach (var col in colliders)
            {
                if (!col.TryGetComponent<GridCell>(out var cell)) continue;
                if (!cell.IsCurrentlyOnFire()) continue;
                _pointToRunAwayFrom = cell.transform;
                _choosePoint = true;
                return;
            }
        }
    }
}