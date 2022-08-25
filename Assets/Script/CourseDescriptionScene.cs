using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CourseDescriptionScene : MonoBehaviour
{
    [SerializeField] private GameObject icon;
    [SerializeField] private Text titleCourse;
    [SerializeField] private Text descriptionCourse;
    [SerializeField] private Image iconCourse;
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject prefabLesson;
    [SerializeField] private GameObject button;
    [SerializeField] private ObjectProfiles objectProfiles;
    private Text textButton;

    private void Awake()
    {
        textButton = button.GetComponentInChildren<Text>();

        icon.GetComponentInChildren<Image>().sprite = CurrentProfile.icon;
        icon.GetComponentInChildren<Text>().text = CurrentProfile.name;
        titleCourse.text = CurrentProfile.currentCourse.title;
        descriptionCourse.text = CurrentProfile.currentCourse.description;
        iconCourse.sprite = CurrentProfile.currentCourse.icon;

        for (int i = 0; i < CurrentProfile.currentCourse.lessons.Count; i++)
        {
            MyLesson lesson = CurrentProfile.currentCourse.lessons[i];
            var objLesson = Instantiate(prefabLesson);
            objLesson.GetComponentInChildren<Text>().text = string.Format("Урок {0}. {1}", i + 1, lesson.title);
            objLesson.transform.SetParent(content.transform, false);
        }

        if (CurrentProfile.courses.Exists(x => x.title.Contains(CurrentProfile.currentCourse.title)))
        {
            var course = CurrentProfile.courses.Find(x => x.title.Contains(CurrentProfile.currentCourse.title));
            if (course.finish)
                textButton.text = "Просмотреть курс";
            else
                textButton.text = "Продолжить курс";
        }
        else
        {
            textButton.text = "Начать курс";
            button.GetComponentInChildren<Button>().onClick.AddListener(() => objectProfiles.AddCourse(CurrentProfile.currentCourse));
        }
        button.GetComponentInChildren<Button>().onClick.AddListener(() => SceneManager.LoadScene("Course"));
    }
}
