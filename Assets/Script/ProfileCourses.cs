using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
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

        foreach (var course in CurrentProfile.courses)
        {
            if (!course.finish)
            {
                var courseObj = Instantiate(prefabCourse);
                var folderText = courseObj.transform.GetChild(0).GetChild(1);
                //courseObj.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite = course.icon;
                StartCoroutine(LoadTextureFromServer(courseObj, course));
                folderText.GetChild(0).GetComponent<Text>().text = course.title;
                var indicator = courseObj.transform.GetChild(1);
                foreach (var lesson in course.lessons)
                {
                    var lineObj = Instantiate(prefabLine);
                    if (lesson.finish)
                    {
                        lineObj.GetComponent<Image>().color = Color.green;
                    }
                    else
                    {
                        if (course.lastLesson == null)
                            course.lastLesson = lesson;
                        lineObj.GetComponent<Image>().color = Color.gray;
                    }
                    lineObj.transform.SetParent(indicator);

                }
                folderText.GetChild(1).GetComponent<Text>().text = course.lastLesson.title;

                courseObj.transform.SetParent(activeList.transform, false);
                courseObj.GetComponent<Button>().onClick.AddListener(() => {
                    SceneManager.LoadScene("Course");
                    CurrentProfile.currentCourse = course;
                    });
            }
            else
            {
                var courseObj = Instantiate(prefabCourse);
                var folderText = courseObj.transform.GetChild(0).GetChild(1);
                folderText.GetChild(0).GetComponent<Text>().text = course.title;
                StartCoroutine(LoadTextureFromServer(courseObj, course));
                courseObj.transform.GetChild(0).GetChild(0).GetChild(0).GetComponentInChildren<Image>().sprite = course.icon;
                courseObj.transform.SetParent(finishList.transform, false);
                courseObj.GetComponent<Button>().onClick.AddListener(() => {
                    SceneManager.LoadScene("Course");
                    CurrentProfile.currentCourse = course;
                });
            }
        }

        IEnumerator LoadTextureFromServer(GameObject courseObj, MyCourse course)
        {
            var request = UnityWebRequestTexture.GetTexture(course.pathIcon);
            yield return request.SendWebRequest();
            var texture = DownloadHandlerTexture.GetContent(request);
            var sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
            course.icon = sprite;
            courseObj.transform.GetChild(0).GetChild(0).GetChild(0).GetComponentInChildren<Image>().sprite = course.icon;
            request.Dispose();
        }
        //var line = indicator.transform.GetChild(0);
        /* Цилк перебора */

    }
}

