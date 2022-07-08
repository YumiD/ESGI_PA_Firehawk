using BehaviorTree;
using UnityEngine;

namespace FirehawkAI.Tasks
{
    public class TaskGoTowardLitBranch : Node
    {
        private readonly Transform _transform;
        private readonly Transform _meshTransform;

        public TaskGoTowardLitBranch(Transform transform, Transform meshTransform)
        {
            _transform = transform;
            _meshTransform = meshTransform;
        }

        public override NodeState Evaluate()
        {
            var target = (GameObject)GetData("FoundLitBranch");
            if (target != null)
            {
                if (Vector3.Distance(_meshTransform.position, target.transform.position) > FirehawkBT.DistanceBetweenMeshParent+.1f)
                {
                    Debug.Log("Go toward lit branch");
                    var position = target.transform.position;
                    _transform.position = Vector3.MoveTowards(
                        _transform.position, position, FirehawkBT.Speed * Time.deltaTime);
                }

                State = NodeState.SUCCESS;
                return State;
            }

            State = NodeState.RUNNING;
            return State;
        }
    }
}