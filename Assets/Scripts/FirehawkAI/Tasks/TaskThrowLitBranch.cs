using BehaviorTree;
using UnityEngine;

namespace FirehawkAI.Tasks
{
    public class TaskThrowLitBranch : Node
    {
        private float _waitBeforeNewAction = 3f;
        private float _waitBeforeNewActionCounter;

        private readonly Transform _transform;
        public TaskThrowLitBranch(Transform transform)
        {
            _transform = transform;
        }

        public override NodeState Evaluate()
        {
            var litBranch = (Transform)GetData("litBranch");
            if (_waitBeforeNewActionCounter > 0)
            {
                Debug.Log("Waiting");
                _waitBeforeNewActionCounter -= Time.deltaTime;
                if (_waitBeforeNewActionCounter <= 0)
                {
                    litBranch.gameObject.layer = 9;
                    ClearData("litBranch");
                    State = NodeState.SUCCESS;
                    return State;
                }
            }
            else
            {
                var rigidbodyLitBranch = litBranch.gameObject.GetComponent<Rigidbody>();
                rigidbodyLitBranch.isKinematic = false;
                rigidbodyLitBranch.AddRelativeForce(_transform.forward * 1000f);
                litBranch.SetParent(null);

                _waitBeforeNewActionCounter = _waitBeforeNewAction;
            }
            State = NodeState.FAILURE;
            return State;
        }
    }
}