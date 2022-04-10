using BehaviorTree;
using UnityEngine;

namespace FirehawkAI.Checks
{
    public class CheckIfFire : Node
    {
        public override NodeState Evaluate()
        {
            var t = GetData("fire");

            if (t == null)
            {
                Debug.Log("CheckIfFire");
                State = NodeState.SUCCESS;
                return State;
            }

            State = NodeState.FAILURE;
            return State;
        }
    }
}