using BehaviorTree;
using UnityEngine;

namespace FirehawkAI.Checks
{
    public class CheckIfFire : Node
    {
        public override NodeState Evaluate()
        {
            var target = GetData("fire");
            // var target2 = GetData("litBranch");

            if (target == null)
            {
                Debug.Log("CheckIfFire");
                State = NodeState.SUCCESS;
                return State;
            }

            if (((GameObject)target).TryGetComponent<GridCell>(out var cell))
            {
                if (!cell.IsCurrentlyOnFire())
                {
                    ClearData("fire");
                }
            }

            State = NodeState.FAILURE;
            return State;
        }
    }
}