using BehaviorTree;
using UnityEngine;

namespace FirehawkAI.Checks
{
    public class CheckHaveLitBranch : Node
    {
        public override NodeState Evaluate()
        {
            var t = GetData("litBranch");

            if (t == null)
            {
                Debug.Log("CheckHaveLitBranch");
                State = NodeState.SUCCESS;
                return State;
            }
            
            State = NodeState.FAILURE;
            return State;
        }
    }
}