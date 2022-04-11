using BehaviorTree;
using UnityEngine;

namespace FirehawkAI.Tasks
{
    public class TaskGoTowardLitBranch : Node
    {
        private readonly Transform _transform;

        public TaskGoTowardLitBranch(Transform transform)
        {
            _transform = transform;
        }

        public override NodeState Evaluate()
        {
            var target = (Transform)GetData("FoundLitBranch");
            var target2 = (Transform)GetData("litBranch");

            if (target == null || target2 != null)
            {
                State = NodeState.FAILURE;
                return State;
            }

            if (Vector3.Distance(_transform.position, target.position) > .1f)
            {
                Debug.Log("Go toward lit branch");
                var position = target.position;
                _transform.position = Vector3.MoveTowards(
                    _transform.position, position, FirehawkBT.Speed * Time.deltaTime);
                _transform.LookAt(position);
            }
            else
            {
                State = NodeState.FAILURE; // But in fact a success, just a way to go to the next task
                return State;
            }

            State = NodeState.RUNNING;
            return State;
        }
    }
}