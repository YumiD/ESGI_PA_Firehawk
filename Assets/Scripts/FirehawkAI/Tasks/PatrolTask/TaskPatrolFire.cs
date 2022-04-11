using BehaviorTree;
using UnityEngine;

namespace FirehawkAI.Tasks.PatrolTask
{
    public class TaskPatrolFire : TaskPatrol
    {
        public TaskPatrolFire(Transform currentTransform, Transform[] waypoints, Vector3 groundTransform) : base(
            currentTransform,
            waypoints, groundTransform)
        {
        }

        public override NodeState Evaluate()
        {
            base.Evaluate();
            var t = GetData("fire");

            if (t == null)
            {
                var colliders = Physics.OverlapSphere(CurrentTransform.position, FirehawkBT.DetectionRange,
                    FirehawkBT.GroundLayerMask);

                if (colliders.Length > 0)
                {
                    foreach (var col in colliders)
                    {
                        if (!col.TryGetComponent<GridCell>(out var cell)) continue;
                        if (!cell.IsCurrentlyOnFire()) continue;
                        Debug.Log("FOUND fire");
                        Parent.Parent.SetData("fire", col.gameObject);
                        State = NodeState.RUNNING;
                        return State;
                    }
                }
            }

            State = NodeState.RUNNING;
            return State;
        }
    }
}