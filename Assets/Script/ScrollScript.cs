using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ScrollScript : MonoBehaviour, IScrollHandler
{
    [SerializeField]
    private GameObject scroll;

    private Coroutine co;
    public void OnScroll(PointerEventData eventData)
    {

        scroll.SetActive(true);
        scroll.GetComponent<Image>().DOFade(1, 0.1f);
        if (!co.IsUnityNull())
            StopCoroutine(co);
        co = StartCoroutine(Active());
    }

    IEnumerator Active()
    {
        yield return new WaitForSeconds(2);
        scroll.GetComponent<Image>().DOFade(0, 0.2f).OnComplete(() => scroll.SetActive(false));
    }

}
