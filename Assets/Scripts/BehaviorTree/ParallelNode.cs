using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class ParallelNode : Node
    {
        public enum Policy
        {
            RequireOne,
            RequireAll
        }

        private readonly Policy _successPolicy;
        private readonly Policy _failurePolicy;

        public ParallelNode(List<Node> children, Policy success, Policy failure) : base(children)
        {
            _successPolicy = success;
            _failurePolicy = failure;
        }

        public override NodeState Evaluate()
        {
            var successCount = 0;
            var failureCount = 0;

            foreach (var node in Children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        failureCount++;
                        if (_failurePolicy == Policy.RequireOne)
                        {
                            State = NodeState.FAILURE;
                            return State;
                        }
                        continue;
                    case NodeState.SUCCESS:
                        successCount++;
                        if (_successPolicy == Policy.RequireOne)
                        {
                            State = NodeState.SUCCESS;
                            return State;
                        }
                        continue;
                    case NodeState.RUNNING:
                        continue;
                    default:
                        State = NodeState.SUCCESS;
                        return State;
                }
            }

            if (_failurePolicy == Policy.RequireAll && failureCount == Children.Count)
            {
                State = NodeState.FAILURE;
                return State;
            }

            if (_successPolicy == Policy.RequireAll && successCount == Children.Count)
            {
                State = NodeState.SUCCESS;
                return State;
            }

            State = NodeState.RUNNING;
            return State;
        }
    }
}