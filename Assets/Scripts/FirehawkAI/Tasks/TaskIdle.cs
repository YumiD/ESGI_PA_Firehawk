using BehaviorTree;
using UnityEngine;

namespace FirehawkAI.Tasks
{
    public class TaskIdle : Node
    {
        public override NodeState Evaluate()
        {
            Debug.Log("Idle");
            State = NodeState.RUNNING;
            return State;
        }
    }
}