using BehaviorTree;
using UnityEngine;

namespace FirehawkAI.Tasks
{
    public class TaskThrowLitBranch : Node
    {
        private float _waitBeforeNewAction = 3f;
        private float _waitBeforeNewActionCounter;

        public override NodeState Evaluate()
        {
            var t = (Transform)GetData("litBranch");
            if (_waitBeforeNewActionCounter > 0)
            {
                Debug.Log("Waiting");
                _waitBeforeNewActionCounter -= Time.deltaTime;
                if (_waitBeforeNewActionCounter <= 0)
                {
                    t.gameObject.layer = 9;
                    ClearData("litBranch");
                    State = NodeState.SUCCESS;
                    return State;
                }
            }
            else
            {
                t.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                t.SetParent(null);

                _waitBeforeNewActionCounter = _waitBeforeNewAction;
                // FirehawkBT.ThrowNewBranch();
            }
            State = NodeState.RUNNING;
            return State;
        }
    }
}