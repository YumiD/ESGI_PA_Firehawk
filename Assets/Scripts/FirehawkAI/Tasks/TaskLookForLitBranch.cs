using BehaviorTree;
using UnityEngine;

namespace FirehawkAI.Tasks
{
    public class TaskLookForLitBranch : TaskPatrol
    {
        public TaskLookForLitBranch(Transform transform, Transform[] waypoints, Vector3 groundTransform) : base(transform, waypoints, groundTransform)
        {
        }

        public override NodeState Evaluate()
        {
            base.Evaluate();
            Debug.Log("Looking for lit branch");
            var t = GetData("litBranch");

            if (t == null)
            {
                var colliders = Physics.OverlapSphere(Transform.position, FirehawkBT.DetectionRange,
                    FirehawkBT.LitBranchLayerMask);

                if (colliders.Length > 0)
                {
                    Debug.Log("FOUND lit branch");
                    Parent.Parent.SetData("branch", colliders[0].transform);

                    State = NodeState.SUCCESS;
                    return State;
                }

                State = NodeState.FAILURE;
                return State;
            }

            State = NodeState.SUCCESS;
            return State;
        }
    }
}