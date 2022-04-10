using BehaviorTree;
using UnityEngine;

namespace FirehawkAI.Tasks
{
    public class TaskLookForLitBranch : TaskPatrol
    {
        public TaskLookForLitBranch(Transform transform, Transform[] waypoints, Vector3 groundTransform) : base(
            transform, waypoints, groundTransform)
        {
        }

        public override NodeState Evaluate()
        {
            var t = GetData("litBranch");
            var t2 = GetData("FoundLitBranch");

            if (t != null || t2 != null)
            {
                State = NodeState.FAILURE;
                return State;
            }
            base.Evaluate();

            Debug.Log("Looking for lit branch");
            var colliders = Physics.OverlapSphere(Transform.position, FirehawkBT.DetectionRange,
                FirehawkBT.LitBranchLayerMask);

            if (colliders.Length > 0)
            {
                Debug.Log("FOUND lit branch");

                Parent.Parent.SetData("FoundLitBranch", colliders[0].transform);
                State = NodeState.FAILURE;
                return State;
            }

            State = NodeState.RUNNING;
            return State;
        }
    }
}