using BehaviorTree;
using UnityEngine;

namespace FirehawkAI.Checks
{
    public class CheckBurrowNotFound : Node
    {
        private Transform _transform;

        public CheckBurrowNotFound(Transform transform)
        {
            _transform = transform;
        }
        public override NodeState Evaluate()
        {
            var fire = GetData("fire");
            if (fire == null)
            {
                State = NodeState.FAILURE;
                return State;
            }
            
            var burrow = GetData("burrow");
            if (burrow == null)
            {
                Debug.Log("Looking for burrow");
                
                var colliders = Physics.OverlapSphere(_transform.position, FirehawkBT.DetectionRange,
                    FirehawkBT.BurrowLayerMask);

                if (colliders.Length > 0)
                {
                    Debug.Log("FOUND Burrow");
                    Parent.Parent.SetData("burrow", colliders[0].transform);

                    State = NodeState.SUCCESS;
                    return State;
                }
                State = NodeState.FAILURE;
                return State;
            }

            State = NodeState.SUCCESS;
            return State;
        }
    }
}