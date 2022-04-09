using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class RandomSequence : Node
    {
        public RandomSequence(List<Node> children) : base(children)
        {
            Children = ShuffleList(Children);
        }
        
        public override NodeState Evaluate()
        {
            foreach (var node in Children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        continue;
                    case NodeState.SUCCESS:
                        State = NodeState.SUCCESS;
                        Children = ShuffleList(Children);
                        return State;
                    case NodeState.RUNNING:
                        State = NodeState.RUNNING;
                        return State;
                    default:
                        continue;
                }
            }

            State = NodeState.FAILURE;
            return State;
        }
    }
}