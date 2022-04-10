using BehaviorTree;
using UnityEngine;

namespace FirehawkAI.Tasks
{
    public class TaskThrowLitBranch : Node
    {
        public override NodeState Evaluate()
        {
            var t = (Transform)GetData("litBranch");
            
            t.GetComponent<Rigidbody>().isKinematic = false;
            t.SetParent(null);

            State = NodeState.SUCCESS;
            return State;
        }
    }
}