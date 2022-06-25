using BehaviorTree;
using FireCellScripts;
using UnityEngine;

namespace FirehawkAI.Checks
{
    public class CheckHoldingBranch : Node
    {
        private readonly Transform _transform;
        private readonly Transform _meshTransform;
        private readonly float _originCoordinate;

        public CheckHoldingBranch(Transform transform, float originCoordinate, Transform meshTransform)
        {
            _transform = transform;
            _meshTransform = meshTransform;
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

            var fireCell = foundLitBranch.GetComponentInChildren<FireCell>();
            if (fireCell != null)
            {
                if (fireCell.FireState == FireState.Smoking)
                {
                    ClearData("FoundLitBranch");
                    State = NodeState.FAILURE;
                    return State;
                }
            }

            var holdingBranch = GetData("isHoldingBranch");
            if (holdingBranch == null)
            {
                var litBranch = GetData("litBranch");
                // Debug.Log("CheckHoldingBranch");
                var currentPos = _transform.position;

                if (litBranch == null)
                {
                    // Go toward lit branch
                    if (Vector3.Distance(_meshTransform.position, foundLitBranch.transform.position) >
                        FirehawkBT.DistanceBetweenMeshParent+.1f)
                    {
                        // Debug.Log("Check go toward lit branch");
                        State = NodeState.SUCCESS;
                        return State;
                    }

                    // Pick Up part
                    var posWithOffset = new Vector3(currentPos.x, currentPos.y + FirehawkBT.YOffset, currentPos.z);
                    var colliders = Physics.OverlapSphere(posWithOffset, FirehawkBT.PickUpRange,
                        FirehawkBT.LitBranchLayerMask);
                    if (colliders.Length > 0)
                    {
                        // Debug.Log("Pick up lit branch");
                        SetDataToRoot("litBranch", colliders[0].transform);
                        if (fireCell != null)
                        {
                            fireCell.HoldByFirehawk();
                        }
                    }
                }

                // Go back in sky part
                if (litBranch != null)
                {
                    // Debug.Log("Go back in sky.");
                    var target = new Vector3(currentPos.x, _originCoordinate, currentPos.z);
                    if (Vector3.Distance(_transform.position, target) <= 0.01f)
                    {
                        // Debug.Log("Go back in sky done");
                        SetDataToRoot("isHoldingBranch", true);
                        ClearData("FoundLitBranch");
                    }

                    State = NodeState.SUCCESS;
                    return State;
                }


                State = NodeState.SUCCESS;
                return State;
            }

            State = NodeState.FAILURE;
            return State;
        }
    }
}