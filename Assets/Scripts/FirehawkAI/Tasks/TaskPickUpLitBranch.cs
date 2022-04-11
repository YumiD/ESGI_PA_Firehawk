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
            var target = (Transform)GetData("litBranch");

            if (target != null)
            {
                State = NodeState.FAILURE;
                return State;
            }
            
            var colliders = Physics.OverlapSphere(_transform.position, FirehawkBT.PickUpRange,
                FirehawkBT.LitBranchLayerMask);

            if (colliders.Length > 0)
            {
                Debug.Log("Pick up lit branch");
                colliders[0].transform.SetParent(_transform);
                colliders[0].GetComponent<Rigidbody>().isKinematic = true;
                // Parent.Parent.SetData("litBranch" + FirehawkBT.CurrentLitBranch, colliders[0].transform);
                SetDataToRoot("litBranch", colliders[0].transform);
                ClearData("FoundLitBranch");

                State = NodeState.FAILURE;
                return State;
            }

            State = NodeState.RUNNING;
            return State;
        }
    }
}