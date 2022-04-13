using BehaviorTree;
using UnityEngine;

namespace FirehawkAI.Tasks
{
    public class TaskPatrol : Node
    {
        protected readonly Transform CurrentTransform;
        private readonly Transform[] _waypoints;

        // private int _currentWaypointIndex;

        public TaskPatrol(Transform currentTransform, Transform[] waypoints)
        {
            CurrentTransform = currentTransform;
            _waypoints = waypoints;
        }

        public override NodeState Evaluate()
        {
            Debug.Log("PATROL");
            var wp = _waypoints[FirehawkBT.CurrentWaypointIndex];
            if (Vector3.Distance(CurrentTransform.position, wp.position) < 0.01f)
            {
                CurrentTransform.position = wp.position;
                // _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
                FirehawkBT.GoToNextWaypoint((FirehawkBT.CurrentWaypointIndex + 1) % _waypoints.Length);
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