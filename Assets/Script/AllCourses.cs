using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
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
    private List<GameObject> listCourse = new List<GameObject>();
    private void Awake()
    {
        objectProfiles = FindObjectOfType<ObjectProfiles>();

        DirectoryInfo dir = new DirectoryInfo(Path.Combine(Application.streamingAssetsPath, "Courses"));
        foreach (var folder in dir.GetDirectories())
        {
            foreach (var file in folder.GetFiles())
            {
                if (file.Extension == ".json")
                {
                    string jsonString = File.ReadAllText(file.FullName);
                    var course = JsonUtility.FromJson<Course>(jsonString);

                    if (CurrentProfile.courses.Exists(x => x.title.Contains(course.title)))
                    {
                        var myCourse = CurrentProfile.courses.Find(x => x.title.Contains(course.title));
                        /* Если курс уже прошли */
                        if (myCourse.finish)
                        {
                            var objCardStart = Instantiate(cardComplite);
                            StartCoroutine(LoadTextureFromServer(Path.Combine(folder.FullName, course.pathIcon), objCardStart, course));
                            var infoCourse = objCardStart.transform.GetChild(1);
                            infoCourse.GetChild(0).GetComponentInChildren<Text>().text = course.title;
                            infoCourse.GetChild(1).GetComponentInChildren<Text>().text = course.lessons.Count.ToString() + " уроков";
                            objCardStart.transform.SetParent(content.transform, false);
                            objCardStart.transform.GetChild(2).GetComponentInChildren<Button>().onClick.AddListener(() => CurrentProfile.currentCourse = new MyCourse(course));
                            listCourse.Add(objCardStart);
                        }
                        /* Если курс уже начали */
                        else
                        {
                            var objCardStart = Instantiate(cardStart);
                            StartCoroutine(LoadTextureFromServer(Path.Combine(folder.FullName, course.pathIcon), objCardStart, course));
                            var infoCourse = objCardStart.transform.GetChild(1);
                            infoCourse.GetChild(0).GetComponentInChildren<Text>().text = course.title;
                            infoCourse.GetChild(1).GetChild(0).GetComponentInChildren<Text>().text = course.lessons.Count.ToString() + " уроков";
                            infoCourse.GetChild(1).GetChild(1).GetComponentInChildren<Text>().text = myCourse.lessons.FindAll(x => x.finish).Count.ToString() + " уроков";
                            objCardStart.transform.SetParent(content.transform, false);
                            objCardStart.transform.GetChild(2).GetComponentInChildren<Button>().onClick.AddListener(
                                () => { 
                                    CurrentProfile.currentCourse = new MyCourse(course); 
                                    /* Переход на другую сцену */
                                });
                            listCourse.Add(objCardStart);
                        }
                    }
                    /* Если не было ещё такого курса */
                    else
                    {
                        var objCardStart = Instantiate(cardNotStart);
                        StartCoroutine(LoadTextureFromServer(Path.Combine(folder.FullName, course.pathIcon), objCardStart, course));
                        var infoCourse = objCardStart.transform.GetChild(1);
                        infoCourse.GetChild(0).GetComponentInChildren<Text>().text = course.title;
                        infoCourse.GetChild(1).GetComponentInChildren<Text>().text = course.lessons.Count.ToString() + " уроков";
                        objCardStart.transform.SetParent(content.transform, false);
                        objCardStart.transform.GetChild(2).GetComponentInChildren<Button>().onClick.AddListener(() => OnClickButtonStart(course));
                        listCourse.Add(objCardStart);
                    }
                }
            }

        }
        //Application.streamingAssetsPath;
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
        foreach (var item in listCourse)
            item.SetActive(true);
        foreach (var item in list)
            item.SetActive(false);
    }

    IEnumerator LoadTextureFromServer(string url, GameObject objCardStart, Course course)
    {
        var request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();
        var texture = DownloadHandlerTexture.GetContent(request);
        course.icon = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        SetSprite(objCardStart, course);
        request.Dispose();
    }
    private void SetSprite(GameObject objCardStart, Course course)
    {
        objCardStart.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = course.icon;
    }

    private void OnClickButtonStart(Course course)
    {
        objectProfiles.AddCourse(course);
        CurrentProfile.currentCourse = new MyCourse(course);
        /* Переход на другую сцену */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
