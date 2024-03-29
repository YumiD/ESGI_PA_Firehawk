﻿using BehaviorTree;
using UnityEngine;

namespace FirehawkAI.Tasks
{
    public class TaskGoBackInSky : Node
    {
        private readonly Transform _transform;
        private readonly float _originCoordinate;

        public TaskGoBackInSky(Transform transform,float originCoordinate)
        {
            _transform = transform;
            _originCoordinate = originCoordinate;
        }
        public override NodeState Evaluate()
        {
            var litBranch = GetData("litBranch");
            if (litBranch == null)
            {
                // Debug.Log("Can't go back yet");
                State = NodeState.SUCCESS;
                return State;
            }
            var currentPos = _transform.position;
            var target = new Vector3(currentPos.x, _originCoordinate, currentPos.z);
            if (Vector3.Distance(_transform.position, target) > 0.01f)
            {
                _transform.position = Vector3.MoveTowards(
                    _transform.position, target, FirehawkBT.Speed * Time.deltaTime);
            }
            
            State = NodeState.RUNNING;
            return State;
        }
    }
}