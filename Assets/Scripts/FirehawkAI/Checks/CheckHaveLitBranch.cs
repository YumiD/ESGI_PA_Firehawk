using BehaviorTree;
using UnityEngine;

namespace FirehawkAI.Checks
{
    public class CheckHaveLitBranch : Node
    {
        private readonly Transform _transform;

        public CheckHaveLitBranch(Transform transform)
        {
            _transform = transform;
        }
        public override NodeState Evaluate()
        {
            var t = GetData("litBranch");

            if (t == null)
            {
                Debug.Log("CheckHaveLitBranch");
                State = NodeState.SUCCESS;
                return State;
            }

            var currentPos = _transform.position;
            var target = new Vector3(currentPos.x, (float)GetData("OriginCoordinate"), currentPos.z);
            if (Vector3.Distance(_transform.position, target) > 0.01f)
            {
                State = NodeState.SUCCESS;
                return State;
            }

            State = NodeState.FAILURE;
            return State;
        }
    }
}