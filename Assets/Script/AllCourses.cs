using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AllCourses : MonoBehaviour
{
    [Header("Cards Course")]
    [SerializeField]
    private GameObject cardNotStart;
    [SerializeField]
    private GameObject cardStart;
    [SerializeField]
    private GameObject cardComplite;

    [Space(10)]
    [SerializeField]
    private GameObject icon;
    [SerializeField]
    private GameObject content;
    [SerializeField]
    private InputField inputSearch;
    private ObjectProfiles objectProfiles;
    private readonly List<GameObject> listCourse = new List<GameObject>();
    private void Awake()
    {
        objectProfiles = FindObjectOfType<ObjectProfiles>();

        DirectoryInfo dir = new DirectoryInfo(Path.Combine(Application.streamingAssetsPath, "Courses"));
        foreach (var (folder, file) in dir.GetDirectories().SelectMany(folder => folder.GetFiles().Where(file => file.Extension == ".json").Select(file => (folder, file))))
        {
            string jsonString = File.ReadAllText(file.FullName);
            var course = JsonUtility.FromJson<Course>(jsonString);
            course.pathIcon = Path.Combine(folder.FullName, course.pathIcon);
            GameObject objCard = null;
            if (CurrentProfile.courses.Exists(x => x.title.Contains(course.title)))
            {
                var myCourse = CurrentProfile.courses.Find(x => x.title.Contains(course.title));
                /* Если курс уже прошли */
                if (myCourse.finish)
                {
                    objCard = Instantiate(cardComplite);
                    StartCoroutine(LoadTextureFromServer(course.pathIcon, objCard, course));
                    var infoCourse = objCard.transform.GetChild(1);
                    infoCourse.GetChild(0).GetComponentInChildren<Text>().text = course.title;
                    infoCourse.GetChild(1).GetComponentInChildren<Text>().text = course.lessons.Count.ToString() + " уроков";
                }
                /* Если курс уже начали */
                else
                {
                    objCard = Instantiate(cardStart);
                    StartCoroutine(LoadTextureFromServer(course.pathIcon, objCard, course));
                    var infoCourse = objCard.transform.GetChild(1);
                    infoCourse.GetChild(0).GetComponentInChildren<Text>().text = course.title;
                    infoCourse.GetChild(1).GetChild(0).GetComponentInChildren<Text>().text = course.lessons.Count.ToString() + " уроков";
                    infoCourse.GetChild(1).GetChild(1).GetComponentInChildren<Text>().text = myCourse.lessons.FindAll(x => x.finish).Count.ToString() + " уроков";
                }
            }
            /* Если не было ещё такого курса */
            else
            {
                objCard = Instantiate(cardNotStart);
                StartCoroutine(LoadTextureFromServer(course.pathIcon, objCard, course));
                var infoCourse = objCard.transform.GetChild(1);
                infoCourse.GetChild(0).GetComponentInChildren<Text>().text = course.title;
                infoCourse.GetChild(1).GetComponentInChildren<Text>().text = course.lessons.Count.ToString() + " уроков";
                objCard.transform.GetChild(2).GetComponentInChildren<Button>().onClick.AddListener(() => objectProfiles.AddCourse(course));
            }
            objCard.GetComponent<Button>().onClick.AddListener(() =>
                {
                    CurrentProfile.currentCourse = new MyCourse(course);
                    SceneManager.LoadScene("CourseDescription");
                });
                objCard.transform.GetChild(2).GetComponentInChildren<Button>().onClick.AddListener(() =>
                            {
                                CurrentProfile.currentCourse = new MyCourse(course);
                                SceneManager.LoadScene("Course");
                            });
            objCard.transform.SetParent(content.transform, false);
            listCourse.Add(objCard);

            }
        inputSearch.onValueChanged.AddListener((value) => FilterCard(value));
    }
    void Start()
    {
        icon.GetComponent<Image>().sprite = CurrentProfile.icon;
        icon.GetComponentInChildren<Text>().text = CurrentProfile.name;
    }

    private void FilterCard(string value)
    {
        var list = listCourse.FindAll(x => !x.transform.GetChild(1).GetChild(0).GetComponentInChildren<Text>().text.Contains(value, StringComparison.CurrentCultureIgnoreCase));
        listCourse.ForEach(x => x.SetActive(true));
        list.ForEach(x => x.SetActive(false));
    }

    IEnumerator LoadTextureFromServer(string url, GameObject objCard, Course course)
    {
        var request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();
        var texture = DownloadHandlerTexture.GetContent(request);
        SetSprite(objCard, texture, course);
        request.Dispose();
    }
    private void SetSprite(GameObject objCard, Texture2D texture, Course course)
    {
        Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        course.icon = sprite;
        objCard.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = sprite;
    }
}
