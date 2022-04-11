using System.Collections.Generic;
using BehaviorTree;
using FirehawkAI.Checks;
using FirehawkAI.Tasks;
using FirehawkAI.Tasks.PatrolTask;
using UnityEngine;
using Tree = BehaviorTree.Tree;

namespace FirehawkAI
{
    public class FirehawkBT : Tree
    {
        [SerializeField] private Transform[] waypoints;
        private Vector3 _groundTransform;
        public const int GroundLayerMask = 1 << 6;
        public const int BurrowLayerMask = 1 << 7;
        public const int LitBranchLayerMask = 1 << 8;
        public const float DetectionRange = 20f;
        public const float PickUpRange = 2f;
        public const float Speed = 24f;
        public static int PreviousLitBranch;
        public static int CurrentLitBranch;

        protected override Node SetupTree()
        {
            CurrentLitBranch++;
            _groundTransform = new Vector3(30f, 0f, 30f);
            var currentPos = transform;
            Node node = new Selector(
                new List<Node>
                {
                    new Sequence(new List<Node>
                    {
                        new CheckIfFire(),
                        new TaskPatrolFire(currentPos, waypoints, _groundTransform)
                    }),
                    new Sequence(new List<Node>
                    {
                        new CheckBurrowNotFound(),
                        new ParallelNode(new List<Node>
                        {
                            new TaskLookForBurrow(currentPos),
                            new TaskPatrol(currentPos, waypoints, _groundTransform)
                        }, ParallelNode.Policy.RequireOne, ParallelNode.Policy.RequireOne)
                    }),
                    new Sequence(new List<Node>
                    {
                        new CheckHaveLitBranch(currentPos),
                        new Selector(new List<Node>
                        {
                            new ParallelNode(new List<Node>
                            {
                                new TaskLookForLitBranch(currentPos),
                                new TaskPatrol(currentPos, waypoints, _groundTransform)
                            }, ParallelNode.Policy.RequireOne, ParallelNode.Policy.RequireOne),
                            new Selector(new List<Node>
                            {
                                new TaskGoTowardLitBranch(currentPos),
                                new TaskPickUpLitBranch(currentPos)
                            }),
                            new TaskGoBackInSky(currentPos)
                        })
                    }),
                    new Sequence(new List<Node>
                    {
                        new CheckUnburnArea(currentPos),
                        new ParallelNode(new List<Node>
                        {
                            new TaskGoTowardUnburnArea(currentPos, waypoints, _groundTransform),
                            new TaskThrowLitBranch()
                        }, ParallelNode.Policy.RequireOne, ParallelNode.Policy.RequireOne)
                    }),
                    new TaskIdle()
                });

            node.SetData("OriginCoordinate", currentPos.position.y);
            return node;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, DetectionRange);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, PickUpRange);
        }

        public static void ThrowNewBranch()
        {
            PreviousLitBranch++;
            CurrentLitBranch++;
        }
    }
}