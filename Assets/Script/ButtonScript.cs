using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerClickHandler
{
    private Vector2 pos;
    private Tween click, hover;

    private void Start()
    {
        StartCoroutine(Sleep());
    }


    IEnumerator Sleep()
    {
        yield return new WaitForSeconds(0.5f);
        pos = GetComponent<RectTransform>().localPosition;
        hover = transform.DOLocalMove(pos + Vector2.up, 0.2f).SetLoops(-1, LoopType.Yoyo).Pause();
        click = transform.DOScale(new Vector2(0.8f, 0.8f), 0.2f).SetLoops(2, LoopType.Yoyo).SetAutoKill(false).Pause();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        hover.Play();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hover.Pause();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!click.IsPlaying())
            click.Restart();
    }
}
