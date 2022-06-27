using System;
using DG.Tweening;
using UnityEngine;

public class Direction : MonoBehaviour
{
    public void Start()
    {
        transform.DOMoveY(transform.position.y - .5f, .7f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDisable()
    {
        transform.DOKill();
    }
}