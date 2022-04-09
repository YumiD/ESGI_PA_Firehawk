using BehaviorTree;

namespace FirehawkAI.Checks
{
    public class CheckHaveLitBranch : Node
    {
        public override NodeState Evaluate()
        {
            var t = GetData("litBranch");

            if (t == null)
            {
                State = NodeState.SUCCESS;
                return State;
            }
            
            State = NodeState.FAILURE;
            return State;
        }
    }
}