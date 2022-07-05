using DG.Tweening;
using UnityEngine;

namespace Trees
{
    public class TreeDirection : MonoBehaviour
    {
        public void Start()
        {
            transform.DOMoveY(transform.position.y - .5f, .7f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        }

        private void OnDestroy()
        {
            transform.DOKill(true);
        }
    }
}