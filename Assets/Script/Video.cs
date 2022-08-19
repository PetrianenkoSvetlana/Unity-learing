using DG.Tweening;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using YoutubeExtractor;

public class Video : MonoBehaviour
{
    [Serializable]
    private class PlayOrPause
    {
        public GameObject _object;
        public Image button;
        public Sprite play;
        public Sprite pause;
    }

    [Serializable]
    private class Volume
    {
        public Image button;
        public Sprite on;
        public Sprite off;
        public Slider sliderVolume;
    }

    [Serializable]
    private class Scale
    {
        public Image button;
        public Sprite on;
        public Sprite off;
    }

    [SerializeField]
    private PlayOrPause playOrPause;
    [SerializeField]
    private Volume volume;
    [SerializeField]
    private Scale scale;

    [SerializeField]
    private GameObject line;
    [SerializeField]
    private GameObject windowLoading;
    [SerializeField]
    private Text currentTime;
    [SerializeField]
    private Text allTime;
    [SerializeField]
    private VideoPlayer vPlayer;
    [SerializeField]
    private Slider sliderVideo;
    [SerializeField]
    private GameObject viewPort;
    [SerializeField]
    private GameObject titleLesson;

    private bool scaling = false;
    private float volumeValue;
    private Image spritePlayOrPause;
    private RectTransform rectTransform;
    private Rect rect;
    private Coroutine co;
    private Vector3 positionCursor;
    private BoxCollider2D boxCollider;

    void Start()
    {
        windowLoading.SetActive(true);
        rectTransform = GetComponent<RectTransform>();
        boxCollider = GetComponentInChildren<BoxCollider2D>();
        spritePlayOrPause = playOrPause._object.GetComponent<Image>();
        vPlayer.Prepare();
        //vPlayer.
        vPlayer.prepareCompleted += OnPrepareFinished;
        vPlayer.started += OnStarted;
        volume.sliderVolume.onValueChanged.AddListener(value => ChangeAudio2(value));
        //vPlayer.frameReady += OnFrameReady;
    }

    private void OnStarted(VideoPlayer source)
    {
        print("Ыефке");
    }

    private void Update()
    {
        var time = vPlayer.time;
        var hour = (int)time / 60 / 60;
        var minute = (int)time / 60;
        var second = (int)time % 60;
        if (hour != 0)
        {
            currentTime.text = string.Format("{0:D2}:{1:D2}:{2:D2} ", hour, minute, second);
        }
        else
            currentTime.text = string.Format("{0:D2}:{1:D2} ", minute, second);
    }

    void OnPrepareFinished(VideoPlayer player)
    {

        sliderVideo.maxValue = vPlayer.frameCount / vPlayer.frameRate;
        var time = sliderVideo.maxValue;
        var hour = (int)time / 60 / 60;
        var minute = (int)time / 60;
        var second = (int)time % 60;
        if (hour != 0)
        {
            allTime.text = string.Format("/ {0:D2}:{1:D2}:{2:D2}", hour, minute, second);
        }
        else
            allTime.text = string.Format("/ {0:D2}:{1:D2}", minute, second);

        sliderVideo.onValueChanged.AddListener(delegate { ChangeVideo(sliderVideo.value); });
        sliderVideo.value = 0;
        windowLoading.transform.GetChild(0).gameObject.SetActive(false);
        windowLoading.transform.GetChild(1).gameObject.SetActive(true);
        //vPlayer.Play();
    }

    private void ChangeVideo(float value)
    {
        vPlayer.time = value;
    }

    public void ChangeAudio2(float value)
    {
        if (value == 0)
        {
            volume.button.sprite = volume.off;
            //volumeValue = 0.1f;
        }
        else
        {
            volume.button.sprite = volume.on;
        }
        vPlayer.SetDirectAudioVolume(0, value);
    }

    public void ChangeAudio()
    {
        if (vPlayer.GetDirectAudioVolume(0) != 0)
        {
            volumeValue = volume.sliderVolume.value;
            volume.sliderVolume.value = 0;
            //volume.button.sprite = volume.off;
        }
        else
        {
            volume.sliderVolume.value = volumeValue;
            //volume.button.sprite = volume.on;
        }
    }
    public void OnClick()
    {
        //spritePlayOrPause.DOFade(1, .1f).From();
        if (vPlayer.isPlaying)
        {
            vPlayer.Pause();
            playOrPause.button.sprite = playOrPause.play;
            spritePlayOrPause.sprite = playOrPause.pause;
        }
        else
        {
            vPlayer.Play();
            playOrPause.button.sprite = playOrPause.pause;
            spritePlayOrPause.sprite = playOrPause.play;
        }
    }
    /// <summary>
    /// Функция срабатывает при нажатии на кнопку Масштабирование
    /// </summary>
    public void ScalingVideo()
    {
        if (!scaling)
        {
            rect = rectTransform.rect;
            viewPort.GetComponent<Mask>().enabled = false;
            titleLesson.SetActive(false);
            Sequence sequenceScaling = DOTween.Sequence();
            sequenceScaling.Append(rectTransform.DOMove(transform.localPosition, .5f)).
                Insert(0, rectTransform.DOSizeDelta(new Vector2(1366, 768), .5f)).OnComplete(() => scaling = !scaling);
            boxCollider.size = new Vector2(1366, 768);
            scale.button.sprite = scale.off;
        }
        else
        {
            viewPort.GetComponent<Mask>().enabled = true;
            titleLesson.SetActive(true);
            Sequence sequenceNormal = DOTween.Sequence();
            sequenceNormal.Append(rectTransform.DOLocalMove(Vector2.zero, .5f)).
                Insert(0, rectTransform.DOSizeDelta(rect.size, .5f)).OnComplete(() => scaling = !scaling);
            boxCollider.size = rect.size;
            scale.button.sprite = scale.on;
        }
    }
    public void ClickOnScreen()
    {
        OnClick();
        Sequence sequences = DOTween.Sequence();
        sequences.Append(playOrPause._object.transform.DOScale(new Vector3(1f, 1f), .1f).From()).
            Append(spritePlayOrPause.DOFade(1, .1f).From()).
            Append(playOrPause._object.transform.DOScale(new Vector3(1.3f, 1.3f), .5f)).
            Insert(0, spritePlayOrPause.DOFade(0, sequences.Duration()));
    }

    private void OnMouseOver()
    {
        if (positionCursor != Input.mousePosition)
        {
            positionCursor = Input.mousePosition;
            line.SetActive(true);
            foreach (var image in line.GetComponentsInChildren<Image>())
                image.DOFade(1, .1f);
            if (co != null)
                StopCoroutine(co);
            co = StartCoroutine(Active());
        }
    }
    IEnumerator Active()
    {
        yield return new WaitForSeconds(3);
        foreach (var image in line.GetComponentsInChildren<Image>())
            image.DOFade(0, .5f);
        line.GetComponent<Image>().DOFade(0, .5f).OnComplete(() => line.SetActive(false));
    }
}
