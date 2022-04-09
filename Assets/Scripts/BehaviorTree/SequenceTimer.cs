using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BehaviorTree
{
    public class SequenceTimer : Node
    {
        private readonly float _timeToWait;
        private float _timeToWaitCounter;
        private readonly bool _isRandom;

        public SequenceTimer()
        {
        }

        public SequenceTimer(List<Node> children, float timeToWait, bool isRandom) : base(children)
        {
            _timeToWait = timeToWait;
            _isRandom = isRandom;
            if (_isRandom)
                Children = ShuffleList(Children);
        }

        public override NodeState Evaluate()
        {
            if (_timeToWaitCounter > 0)
            {
                _timeToWaitCounter -= Time.deltaTime;
                State = NodeState.RUNNING;
                return State;
            }

            // var anyChildIsRunning = false;

            // foreach (var node in Children.ToList())
            // {
            var node = Children[Random.Range(0, Children.Count)];
            switch (node.Evaluate())
            {
                case NodeState.FAILURE:
                    State = NodeState.FAILURE;
                    return State;
                case NodeState.SUCCESS:
                    _timeToWaitCounter = _timeToWait;
                    if (_isRandom)
                        Children = ShuffleList(Children);
                    // continue;
                    break;
                case NodeState.RUNNING:
                    // anyChildIsRunning = true;
                    break;
                default:
                    State = NodeState.SUCCESS;
                    return State;
            }

            State = NodeState.SUCCESS;
            return State;
            // }

            // State = anyChildIsRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            // return State;
        }
    }
}