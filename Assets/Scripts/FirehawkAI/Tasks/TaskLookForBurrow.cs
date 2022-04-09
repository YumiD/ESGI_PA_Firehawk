using BehaviorTree;
using UnityEngine;

namespace FirehawkAI.Tasks
{
    public class TaskLookForBurrow : Node
    {
        private readonly Transform _transform;

        public TaskLookForBurrow(Transform transform)
        {
            _transform = transform;
        }

        public override NodeState Evaluate()
        {
            var t = GetData("burrow");

            if (t != null)
            {
                State = NodeState.SUCCESS;
                return State;
            }

            var colliders = Physics.OverlapSphere(_transform.position, FirehawkBT.DetectionRange,
                FirehawkBT.BurrowLayerMask);

            if (colliders.Length > 0)
            {
                Debug.Log("FOUND Burrow");
                Parent.Parent.SetData("burrow", colliders[0].transform);

                State = NodeState.SUCCESS;
                return State;
            }

            State = NodeState.RUNNING;
            return State;
        }
    }
}