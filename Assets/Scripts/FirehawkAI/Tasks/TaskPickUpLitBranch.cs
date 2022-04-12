using BehaviorTree;
using UnityEngine;

namespace FirehawkAI.Tasks
{
    public class TaskPickUpLitBranch : Node
    {
        private readonly Transform _transform;
        private float _yOffset = 2f;

        public TaskPickUpLitBranch(Transform transform)
        {
            _transform = transform;
        }
        
        public override NodeState Evaluate()
        {
            var currentPos = _transform.position;
            var colliders = Physics.OverlapSphere(new Vector3(currentPos.x ,currentPos.y + _yOffset ,currentPos.z), FirehawkBT.PickUpRange,
                FirehawkBT.LitBranchLayerMask);

            if (colliders.Length > 0)
            {
                colliders[0].transform.SetParent(_transform);
                colliders[0].GetComponent<Rigidbody>().isKinematic = true;

                State = NodeState.SUCCESS;
                return State;
            }

            State = NodeState.RUNNING;
            return State;
        }
    }
}