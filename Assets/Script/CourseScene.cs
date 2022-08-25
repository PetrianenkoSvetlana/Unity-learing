using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class CourseScene : MonoBehaviour
{
    [Serializable]
    private class Lessons
    {
        public GameObject prefabCardFirst;
        public GameObject prefabCard;
        public GameObject prefabCardLast;
        public GameObject prefabCardCompleteFirst;
        public GameObject prefabCardComplete;
        public GameObject prefabCardCompleteLast;
    }

    [SerializeField] private Text titleCourse;
    [SerializeField] private GameObject icon;
    [SerializeField] private Lessons lessons;
    [SerializeField] private GameObject contentLessons;
    [SerializeField] private Text titleLesson;
    [SerializeField] private GameObject objVideoPlayer;
    [SerializeField] private VideoPlayer vPlayer;
    [SerializeField] private GameObject windowLoading;
    [SerializeField] private Text descriptionLesson;

    private void Awake()
    {
        icon.GetComponentInChildren<Image>().sprite = CurrentProfile.icon;
        icon.GetComponentInChildren<Text>().text = CurrentProfile.name;

        titleCourse.text = CurrentProfile.currentCourse.title;

        for (int i = 0; i < CurrentProfile.currentCourse.lessons.Count; i++)
        {
            MyLesson lesson = CurrentProfile.currentCourse.lessons[i];
            GameObject lessonObj = null;
            if (!lesson.finish)
            {
                if (i == 0)
                {
                    lessonObj = Instantiate(lessons.prefabCardFirst);
                }
                else if (i == CurrentProfile.currentCourse.lessons.Count - 1)
                {
                    lessonObj = Instantiate(lessons.prefabCardLast);
                }
                else
                {
                    lessonObj = Instantiate(lessons.prefabCard);
                }
            }
            else
            {
                if (i == 0)
                {
                    lessonObj = Instantiate(lessons.prefabCardCompleteFirst);
                }
                else if (i == CurrentProfile.currentCourse.lessons.Count - 1)
                {
                    lessonObj = Instantiate(lessons.prefabCardCompleteLast);
                }
                else
                {
                    lessonObj = Instantiate(lessons.prefabCardComplete);
                }
            }
            var textInfo = lessonObj.transform.GetChild(1);
            textInfo.GetChild(0).GetComponent<Text>().text = string.Format("Урок {0}.", i + 1);
            textInfo.GetChild(1).GetComponent<Text>().text = lesson.title;
            lessonObj.GetComponentInChildren<Button>().onClick.AddListener(() => { SelectCourse(lesson); lessonObj.GetComponentInChildren<Button>().interactable = false; });
            lessonObj.transform.SetParent(contentLessons.transform, false);
        }

        var lessonFirstUnfinish = CurrentProfile.currentCourse.lessons.Find(x => !x.finish);
        SelectCourse(lessonFirstUnfinish);
    }

    public void SelectCourse(MyLesson lesson)
    {
        foreach (Transform child in contentLessons.transform) {
            child.GetComponentInChildren<Button>().interactable = true;
        }
        vPlayer.Stop();
        windowLoading.SetActive(true);
        windowLoading.transform.GetChild(0).gameObject.SetActive(true);
        windowLoading.transform.GetChild(1).gameObject.SetActive(false);
        if (!(lesson.url == null || lesson.url == ""))
        {
            objVideoPlayer.transform.localScale = Vector2.one;
            vPlayer.url = lesson.url;
            vPlayer.Prepare();
        }
        else
        {
            objVideoPlayer.transform.localScale = Vector2.right;
        }
        titleLesson.text = lesson.title;
        descriptionLesson.text = lesson.description;
        LayoutRebuilder.MarkLayoutForRebuild((RectTransform)objVideoPlayer.transform.parent);
    }

}
