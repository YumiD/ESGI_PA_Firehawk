using BehaviorTree;
using UnityEngine;

namespace FirehawkAI.Checks
{
    public class CheckHoldingBranch : Node
    {
        private readonly Transform _transform;
        private readonly float _originCoordinate;

        public CheckHoldingBranch(Transform transform, float originCoordinate)
        {
            _transform = transform;
            _originCoordinate = originCoordinate;
        }

        public override NodeState Evaluate()
        {
            var foundLitBranch = (GameObject)GetData("FoundLitBranch");
            if (foundLitBranch == null)
            {
                State = NodeState.FAILURE;
                return State;
            }

            var holdingBranch = GetData("isHoldingBranch");
            if (holdingBranch == null)
            {
                Debug.Log("CheckHoldingBranch");
                var litBranch = GetData("litBranch");

                // Go back in sky part
                if (litBranch != null)
                {
                    Debug.Log("Go back in sky.");
                    var currentPos = _transform.position;
                    var target = new Vector3(currentPos.x, _originCoordinate, currentPos.z);
                    if (Vector3.Distance(_transform.position, target) <= 0.01f)
                    {
                        Debug.Log("Go back in sky done");
                        SetDataToRoot("isHoldingBranch", true);
                        ClearData("FoundLitBranch");
                        // State = NodeState.SUCCESS;
                        // return State;
                    }

                    State = NodeState.SUCCESS;
                    return State;
                }

                // Go toward lit branch
                if (Vector3.Distance(_transform.position, foundLitBranch.transform.position) > .1f)
                {
                    State = NodeState.SUCCESS;
                    return State;
                }

                // Pick Up part
                var colliders = Physics.OverlapSphere(_transform.position, FirehawkBT.PickUpRange,
                    FirehawkBT.LitBranchLayerMask);

                if (colliders.Length > 0)
                {
                    Debug.Log("Pick up lit branch");
                    SetDataToRoot("litBranch", colliders[0].transform);
                }
            }

            State = NodeState.FAILURE;
            return State;
        }
    }
}