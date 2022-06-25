using System;
using BehaviorTree;
using UnityEngine;

namespace FirehawkAI.Checks
{
    public class CheckHaveLitBranch : Node
    {
        private readonly Transform _transform;

        public CheckHaveLitBranch(Transform transform)
        {
            _transform = transform;
        }

        public override NodeState Evaluate()
        {
            var burrow = (Transform)GetData("burrow");

            if (burrow == null)
            {
                State = NodeState.FAILURE;
                return State;
            }

            var foundLitBranch = GetData("FoundLitBranch");

            // Debug.Log("CheckHaveLitBranch");
            if (foundLitBranch == null)
            {
                var colliders = Physics.OverlapSphere(_transform.position, FirehawkBT.DetectionRange,
                    FirehawkBT.LitBranchLayerMask);

                if (colliders.Length > 0)
                {
                    foreach (var col in colliders)
                    {
                        // Debug.Log("FOUND Found lit branch");
                        Parent.Parent.SetData("FoundLitBranch", col.gameObject);
                        State = NodeState.SUCCESS;
                        return State;
                    }
                }

                State = NodeState.FAILURE;
                return State;
            }

            State = NodeState.SUCCESS;
            return State;

            // var foundLitBranch = (Transform)GetData("FoundLitBranch");
            // var litBranch = GetData("litBranch");
            // var verticalHeight = (float)GetData("OriginCoordinate");
            // var target = _transform.position;
            // if (litBranch == null || foundLitBranch == null || Math.Abs(_transform.position.y - verticalHeight) > .1f)
            // {
            //     Debug.Log("CheckHaveLitBranch");
            //     var collidersBranchDetection = Physics.OverlapSphere(target, FirehawkBT.DetectionRange,
            //         FirehawkBT.LitBranchLayerMask);
            //     var collidersPickUpDetection = Physics.OverlapSphere(target, FirehawkBT.PickUpRange,
            //         FirehawkBT.LitBranchLayerMask);
            //     if (collidersBranchDetection.Length > 0)
            //     {
            //         Debug.Log("FOUND lit branch");
            //
            //         Parent.Parent.SetData("FoundLitBranch", collidersBranchDetection[0].transform);
            //     }
            //
            //     var skyCoordinate = new Vector3(target.x, verticalHeight, target.z);
            //
            //     if (Vector3.Distance(_transform.position, skyCoordinate) > 0.01f)
            //     {
            //         State = NodeState.SUCCESS;
            //         return State;
            //     }
            //
            //     State = NodeState.FAILURE;
            //     return State;
            // }
            //
            // State = NodeState.SUCCESS;
            // return State;
        }
    }
}