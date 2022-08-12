using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingAnimation : MonoBehaviour
{
    private Tween tween;
    private void Start()
    {
        tween = transform.DOLocalRotate(new Vector3(0, 0, -360), 1, RotateMode.FastBeyond360).SetLoops(-1);
    }
    private void OnEnable()
    {
        tween.Play().Restart();
    }

    private void OnDisable()
    {
        tween.Pause();
    }
}
