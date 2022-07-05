using System.Linq;
using DG.Tweening;
using FireCellScripts;
using UnityEngine;

namespace Trees
{
    public class Seed : MonoBehaviour
    {
        [SerializeField] private Transform[] points;
        [SerializeField] private FireCell fireCell;

        public void OnEnable()
        {
            fireCell.DebugSetTemperature(900f);
            Vector3[] positions = new Vector3[3];
            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = points[i].position;
            }
            transform.DOPath(positions, 3f, PathType.CatmullRom).OnComplete(() =>
            {
                gameObject.AddComponent<Rigidbody>();
                var sphere = gameObject.AddComponent<SphereCollider>();
                sphere.radius = .2f;
            });
        }
    }
}