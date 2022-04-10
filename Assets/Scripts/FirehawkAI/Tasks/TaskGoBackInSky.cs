using BehaviorTree;
using UnityEngine;

namespace FirehawkAI.Tasks
{
    public class TaskGoBackInSky : Node
    {
        private readonly Transform _transform;

        public TaskGoBackInSky(Transform transform)
        {
            _transform = transform;
        }
        public override NodeState Evaluate()
        {
            var currentPos = _transform.position;
            var target = new Vector3(currentPos.x, (float)GetData("OriginCoordinate"), currentPos.z);
            if (Vector3.Distance(_transform.position, target) > 0.01f)
            {
                Debug.Log("Go back to sky.");
                _transform.position = Vector3.MoveTowards(
                    _transform.position, target, FirehawkBT.Speed * Time.deltaTime);
                _transform.LookAt(target);
            }
            else
            {
                State = NodeState.FAILURE;
                return State;                
            }
            State = NodeState.RUNNING;
            return State;
        }
    }
}