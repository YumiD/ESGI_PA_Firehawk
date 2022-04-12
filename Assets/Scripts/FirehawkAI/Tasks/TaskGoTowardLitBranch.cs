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
            var target = (GameObject)GetData("FoundLitBranch");
            if (target != null)
            {
                if (Vector3.Distance(_transform.position, target.transform.position) > .1f)
                {
                    Debug.Log("Go toward lit branch");
                    var position = target.transform.position;
                    _transform.position = Vector3.MoveTowards(
                        _transform.position, position, FirehawkBT.Speed * Time.deltaTime);
                    _transform.LookAt(position);
                }   
            }

            State = NodeState.RUNNING;
            return State;
        }
    }
}