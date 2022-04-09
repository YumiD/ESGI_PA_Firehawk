using BehaviorTree;
using UnityEngine;

namespace FirehawkAI.Tasks
{
    public class TaskGoTowardLitBranch : Node
    {
        private Transform _transform;
        public TaskGoTowardLitBranch(Transform transform)
        {
            _transform = transform;
        }
        public override NodeState Evaluate()
        {
            var target = (Transform)GetData("target");

            if (Vector3.Distance(_transform.position, target.position) > 0.01f)
            {
                var position = target.position;
                _transform.position = Vector3.MoveTowards(
                    _transform.position, position, FirehawkBT.Speed * Time.deltaTime);
                _transform.LookAt(position);
            }

            State = NodeState.RUNNING;
            return State;
        }
    }
}