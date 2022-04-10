using BehaviorTree;
using UnityEngine;

namespace FirehawkAI.Tasks
{
    public class TaskPickUpLitBranch : Node
    {
        private readonly Transform _transform;

        public TaskPickUpLitBranch(Transform transform)
        {
            _transform = transform;
        }
        
        public override NodeState Evaluate()
        {
            var colliders = Physics.OverlapSphere(_transform.position, FirehawkBT.PickUpRange,
                FirehawkBT.LitBranchLayerMask);

            if (colliders.Length > 0)
            {
                Debug.Log("Pick up lit branch");
                colliders[0].transform.SetParent(_transform);
                Parent.Parent.SetData("litBranch", colliders[0].transform);
                // ClearData("FoundLitBranch");

                State = NodeState.FAILURE;
                return State;
            }

            State = NodeState.RUNNING;
            return State;
        }
    }
}