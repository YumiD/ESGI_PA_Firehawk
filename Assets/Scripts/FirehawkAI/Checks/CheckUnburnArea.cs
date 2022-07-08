using BehaviorTree;
using FireCellScripts;
using UnityEngine;

namespace FirehawkAI.Checks
{
    public class CheckUnburnArea : Node
    {
        private readonly Transform _transform;

        public CheckUnburnArea(Transform transform)
        {
            _transform = transform;
        }

        public override NodeState Evaluate()
        {
            var holdingBranch = GetData("isHoldingBranch");
            if (holdingBranch == null)
            {
                State = NodeState.FAILURE;
                return State;
            }

            if ((bool)holdingBranch)
            {
                var colliders = Physics.OverlapSphere(_transform.position, FirehawkBT.DetectionRange,
                    FirehawkBT.GrassLayerMask);
                if (colliders.Length > 0)
                {
                    Debug.Log("Looking for unburn area");
                    var countNbOnFireCell = 0;
                    foreach (var col in colliders)
                    {
                        var cell = col.GetComponentInChildren<FireCell>();
                        if (cell != null)
                        {
                            if (cell.FireState != FireState.None)
                            {
                                countNbOnFireCell++;
                            }
                        }
                    }

                    if (countNbOnFireCell < colliders.Length * .1f)
                    {
                        SetDataToRoot("isHoldingBranch", false);
                        State = NodeState.SUCCESS;
                        return State;
                    }
                }
            }

            State = NodeState.FAILURE;
            return State;
        }
    }
}