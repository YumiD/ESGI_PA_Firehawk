using BehaviorTree;
using UnityEngine;

namespace FirehawkAI.Tasks
{
    public class TaskPatrol : Node
    {
        protected readonly Transform Transform;
        protected readonly Vector3 GroundTransform;

        // private readonly Animator _animator;
        private readonly Transform[] _waypoints;

        private int _currentWaypointIndex;

        public TaskPatrol(Transform transform, Transform[] waypoints, Vector3 groundTransform)
        {
            Transform = transform;
            // _animator = transform.GetComponent<Animator>();
            _waypoints = waypoints;
            GroundTransform = groundTransform;
        }

        public override NodeState Evaluate()
        {
            var wp = _waypoints[_currentWaypointIndex];
            if (Vector3.Distance(Transform.position, wp.position) < 0.01f)
            {
                Transform.position = wp.position;
                _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
            }
            else
            {
                var waypointPosition = wp.position;
                Transform.position = Vector3.MoveTowards(Transform.position, waypointPosition,
                    FirehawkBT.Speed * Time.deltaTime);
                Transform.LookAt(waypointPosition);
            }
            State = NodeState.RUNNING;
            return State;
        }
    }
}