using BehaviorTree;
using UnityEngine;

namespace FirehawkAI.Tasks
{
    public class TaskPickUpLitBranch : Node
    {
        private readonly Transform _transform;
        private readonly Transform _feetTransform;

        public TaskPickUpLitBranch(Transform transform, Transform feetTransform)
        {
            _transform = transform;
            _feetTransform = feetTransform;
        }
        
        public override NodeState Evaluate()
        {
            var litBranch = (Transform)GetData("litBranch");
            if (litBranch == null)
            {
                // Debug.Log("Can't pick up");
                State = NodeState.SUCCESS;
                return State;
            }
            var currentPos = _transform.position;
            var posWithOffset = new Vector3(currentPos.x, currentPos.y + FirehawkBT.YOffset, currentPos.z);

            var colliders = Physics.OverlapSphere(posWithOffset, FirehawkBT.PickUpRange,
                FirehawkBT.LitBranchLayerMask);

            if (colliders.Length > 0)
            {
                colliders[0].GetComponent<Rigidbody>().isKinematic = true;
                colliders[0].transform.SetParent(_feetTransform);
                colliders[0].transform.localPosition = Vector3.zero;

                State = NodeState.SUCCESS;
                return State;
            }

            State = NodeState.RUNNING;
            return State;
        }
    }
}