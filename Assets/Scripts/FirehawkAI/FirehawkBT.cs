using System.Collections.Generic;
using BehaviorTree;
using FirehawkAI.Checks;
using FirehawkAI.Tasks;
using Sounds;
using UnityEngine;
using Tree = BehaviorTree.Tree;

namespace FirehawkAI
{
    public class FirehawkBT : Tree
    {
        [SerializeField] private Transform[] waypoints;
        [SerializeField] private Transform meshTransform;
        [SerializeField] private Transform feetTransform;
        public const int GroundLayerMask = 1 << 6;
        public const int GrassLayerMask = 1 << 10;
        public const int BurningLayerMask = 1 << 12;
        public const int BurrowLayerMask = 1 << 7;
        public const int LitBranchLayerMask = 1 << 8;
        public const float DetectionRange = 20f;
        public const float PickUpRange = 1f;
        public const float Speed = 24f;
        public const float YOffset = 1f;
        public static float DistanceBetweenMeshParent;

        public static int CurrentWaypointIndex;
        protected override Node SetupTree()
        {
            var currentPos = transform;
            var originCoordinate = currentPos.position.y;
            DistanceBetweenMeshParent = Vector3.Distance(currentPos.position, meshTransform.position);
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
                        new CheckHoldingBranch(currentPos, originCoordinate, meshTransform),
                        new Sequence(new List<Node>
                        {
                            new TaskGoTowardLitBranch(currentPos, meshTransform),
                            new TaskPickUpLitBranch(currentPos, feetTransform)
                        }),
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
            SoundManager.Instance.LoopSound(10, SoundCategory.Actor,"EagleChirp", false);
            return node;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            var currentPosition = transform.position;
            Gizmos.DrawWireSphere(currentPosition, DetectionRange);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(new Vector3(currentPosition.x, currentPosition.y + YOffset, currentPosition.z),
                PickUpRange);
        }

        public static void GoToNextWaypoint(int nextWaypoint)
        {
            CurrentWaypointIndex = nextWaypoint;
        }
    }
}