using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileCourses : MonoBehaviour
{
    [SerializeField]
    private GameObject infoProfile;
    [SerializeField]
    private GameObject prefabCourse;
    [SerializeField]
    private GameObject prefabLine;
    [SerializeField]
    private GameObject activeList;
    [SerializeField]
    private GameObject finishList;

    private Text nameProfile;
    private Image icon;
    // Start is called before the first frame update
    void Awake()
    {
        infoProfile.GetComponentInChildren<Text>().text = CurrentProfile.name;
        infoProfile.GetComponentInChildren<Image>().sprite = CurrentProfile.icon;

        /*********** Проверка ************/

        var courseTest = new MyCourse();
        courseTest.name = "Мой первый курс";
        courseTest.lessons = new MyLesson[6];
        //courseTest.lastLesson = courseTest.lessons[2];
        courseTest.lessons[0] = new MyLesson();
        courseTest.lessons[0].finish = true;
        courseTest.lessons[1] = new MyLesson();
        courseTest.lessons[1].finish = true;
        courseTest.lessons[2] = new MyLesson();
        courseTest.lessons[2].name = "3. Вроде как третий урок";
        courseTest.lessons[3] = new MyLesson();
        courseTest.lessons[4] = new MyLesson();
        courseTest.lessons[5] = new MyLesson();

        /*********************************/
        /* Нужно заменить */
        //var courses = new string[] { "qe", "asd" };
        //foreach (var course in courses)
        //{
        var courseObj = Instantiate(prefabCourse);
            var folderText = courseObj.transform.GetChild(0).GetChild(1);
            folderText.GetChild(0).GetComponent<Text>().text = courseTest.name;

            var indicator = courseObj.transform.GetChild(1);
        //var line = indicator.transform.GetChild(0);
        /* Цилк перебора */
        foreach (var lesson in courseTest.lessons)
        {
            var lineObj = Instantiate(prefabLine);
            if (lesson.finish)
            {
                lineObj.GetComponent<Image>().color = Color.green;
            }
            else
            {
                if (courseTest.lastLesson == null)
                    courseTest.lastLesson = lesson;
                lineObj.GetComponent<Image>().color = Color.gray;
            }
            lineObj.transform.SetParent(indicator);

        }
        folderText.GetChild(1).GetComponent<Text>().text = courseTest.lastLesson.name;

        courseObj.transform.SetParent(activeList.transform, false);
        //}
    }

}
