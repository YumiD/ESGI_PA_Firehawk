using BehaviorTree;
using UnityEngine;

namespace FirehawkAI.Tasks
{
    public class TaskPatrol : Node
    {
        protected readonly Transform CurrentTransform;
        protected readonly Vector3 GroundTransform;

        // private readonly Animator _animator;
        private readonly Transform[] _waypoints;

        private int _currentWaypointIndex;

        public TaskPatrol(Transform currentTransform, Transform[] waypoints, Vector3 groundTransform)
        {
            CurrentTransform = currentTransform;
            // _animator = transform.GetComponent<Animator>();
            _waypoints = waypoints;
            GroundTransform = groundTransform;
        }

        public override NodeState Evaluate()
        {
            var wp = _waypoints[_currentWaypointIndex];
            if (Vector3.Distance(CurrentTransform.position, wp.position) < 0.01f)
            {
                CurrentTransform.position = wp.position;
                _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
            }
            else
            {
                var waypointPosition = wp.position;
                CurrentTransform.position = Vector3.MoveTowards(CurrentTransform.position, waypointPosition,
                    FirehawkBT.Speed * Time.deltaTime);
                CurrentTransform.LookAt(waypointPosition);
            }
            State = NodeState.RUNNING;
            return State;
        }
    }
}