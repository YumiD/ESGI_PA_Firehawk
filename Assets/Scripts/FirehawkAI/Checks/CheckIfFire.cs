using BehaviorTree;
using UnityEngine;

namespace FirehawkAI.Checks
{
    public class CheckIfFire : Node
    {
        private Transform _transform;
        public CheckIfFire(Transform transform)
        {
            _transform = transform;
        }
        public override NodeState Evaluate()
        {
            var target = GetData("fire");

            if (target == null)
            {
                Debug.Log("CheckIfFire");
                var colliders = Physics.OverlapSphere(_transform.position, FirehawkBT.DetectionRange,
                    FirehawkBT.GroundLayerMask);

                if (colliders.Length > 0)
                {
                    foreach (var col in colliders)
                    {
                        if (!col.TryGetComponent<GridCell>(out var cell)) continue;
                        // if (!cell.IsCurrentlyOnFire()) continue; //TODO: fix
                        Debug.Log("FOUND fire");
                        Parent.Parent.SetData("fire", col.gameObject);
                        State = NodeState.SUCCESS;
                        return State;
                    }
                }

                State = NodeState.FAILURE;
                return State;
            }

            if (((GameObject)target).TryGetComponent<GridCell>(out var currentCell))
            {
                // if (!currentCell.IsCurrentlyOnFire()) //TODO: fix
                {
                    ClearData("fire");
                }
            }

            State = NodeState.SUCCESS;
            return State;
        }
    }
}