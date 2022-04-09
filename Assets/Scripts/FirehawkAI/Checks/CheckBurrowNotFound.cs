using BehaviorTree;
using UnityEngine;

namespace FirehawkAI.Checks
{
    public class CheckBurrowNotFound : Node
    {
        public override NodeState Evaluate()
        {
            var t = GetData("burrow");

            if (t == null)
            {
                Debug.Log("Looking for burrow");
                State = NodeState.SUCCESS;
                return State;
            }
            
            State = NodeState.FAILURE;
            return State;
        }
    }
}