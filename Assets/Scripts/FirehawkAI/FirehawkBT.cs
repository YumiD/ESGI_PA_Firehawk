using System.Collections.Generic;
using BehaviorTree;
using FirehawkAI.Checks;
using FirehawkAI.Tasks;
using UnityEngine;
using Tree = BehaviorTree.Tree;

namespace FirehawkAI
{
    public class FirehawkBT : Tree
    {
        [SerializeField] private Transform[] waypoints;
        public const int GroundLayerMask = 1 << 6;
        public const int BurrowLayerMask = 1 << 7;
        public const int LitBranchLayerMask = 1 << 8;
        public const float DetectionRange = 20f;
        public const float PickUpRange = 1f;
        public const float Speed = 24f;

        public static int CurrentWaypointIndex;
        protected override Node SetupTree()
        {
            var currentPos = transform;
            var originCoordinate = currentPos.position.y;
            Node node = new Selector(
                new List<Node>
                {
                    new Sequence(new List<Node>
                    {
                        new CheckUnburnArea(currentPos),
                        new ParallelNode(new List<Node>
                        {
                            new TaskPatrol(currentPos, waypoints),
                            new TaskThrowLitBranch(currentPos)
                        }, ParallelNode.Policy.RequireOne, ParallelNode.Policy.RequireOne)
                    }),
                    new Sequence(new List<Node>
                    {
                        new CheckHoldingBranch(currentPos, originCoordinate),
                        new ParallelNode(new List<Node>
                        {
                            new TaskGoTowardLitBranch(currentPos),
                            new TaskPickUpLitBranch(currentPos)
                        }, ParallelNode.Policy.RequireOne, ParallelNode.Policy.RequireOne),
                        new TaskGoBackInSky(currentPos, originCoordinate)
                    }),
                    new Sequence(new List<Node>
                    {
                        new CheckHaveLitBranch(currentPos),
                        new TaskPatrol(currentPos, waypoints)
                    }),
                    new Sequence(new List<Node>
                    {
                        new CheckBurrowNotFound(currentPos),
                        new TaskPatrol(currentPos, waypoints)
                    }),
                    new Sequence(new List<Node>
                    {
                        new CheckIfFire(currentPos),
                        new TaskPatrol(currentPos, waypoints)
                    }),
                    new TaskPatrol(currentPos, waypoints)
                });

            // node.SetData("OriginCoordinate", currentPos.position.y);
            // node.SetData("isHoldingBranch", false);

            return node;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            var currentPosition = transform.position;
            Gizmos.DrawWireSphere(currentPosition, DetectionRange);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(new Vector3(currentPosition.x, currentPosition.y + 2f, currentPosition.z),
                PickUpRange);
        }

        public static void GoToNextWaypoint(int nextWaypoint)
        {
            CurrentWaypointIndex = nextWaypoint;
        }
    }
}