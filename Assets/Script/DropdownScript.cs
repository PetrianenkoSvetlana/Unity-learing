using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DropdownScript : MonoBehaviour
{
    [SerializeField]
    private GameObject arrow;
    [SerializeField]
    private GameObject list;

    private float timeAnimation = 0.3f;
    public void ChangeArrow()
    {
        if (list.transform.localScale.y == 1)
        {
            arrow.transform.DORotate(Vector3.zero, timeAnimation);
            list.transform.DOScaleY(0, timeAnimation).OnUpdate(() => LayoutRebuilder.MarkLayoutForRebuild((RectTransform)transform));
        }
        else
        {
            list.SetActive(true);
            arrow.transform.DORotate(Vector3.forward * -180, timeAnimation);
            list.transform.DOScaleY(1, timeAnimation).OnUpdate(() => LayoutRebuilder.MarkLayoutForRebuild((RectTransform)transform));
        }
    }
}
