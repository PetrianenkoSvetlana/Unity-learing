using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] GameObject _myProfile;
    [SerializeField] GameObject _allCourses;
    [SerializeField] GameObject _myCourses;
    [SerializeField] GameObject _setting;
    [SerializeField] GameObject _exitProfile;
    [SerializeField] GameObject _exit;

    [Header("Text")]
    [SerializeField] GameObject _message;

    [Header("Canvas")]
    [SerializeField] GameObject _menuWindow;

    Button _myProfileButton;
    Button _allCoursesButton;
    Button _myCoursesButton;
    Button _settingButton;
    Button _exitProfileButton;
    Button _exitButton;

    Transform _menuTransform;

    private void Awake()
    {
        var objs = GameObject.FindGameObjectsWithTag("Menu");
        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        _myProfileButton = _myProfile.GetComponentInChildren<Button>();
        _myProfileButton.onClick.AddListener(() => _message.SetActive(true));

        _allCoursesButton = _allCourses.GetComponentInChildren<Button>();
        _allCoursesButton.onClick.AddListener(() => OnButtonClick("All—ourses"));

        _myCoursesButton = _myCourses.GetComponentInChildren<Button>();
        _myCoursesButton.onClick.AddListener(() => OnButtonClick("ProfileCourses"));

        _settingButton = _setting.GetComponentInChildren<Button>();
        _settingButton.onClick.AddListener(() => _message.SetActive(true));

        _exitProfileButton = _exitProfile.GetComponentInChildren<Button>();
        _exitProfileButton.onClick.AddListener(() => OnButtonClick("MainScene"));

        _exitButton = _exit.GetComponentInChildren<Button>();
        _exitButton.onClick.AddListener(() => Application.Quit());

        _menuTransform = _menuWindow.transform.GetChild(0);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnButtonClick(string nameScene)
    {
        SceneManager.LoadScene(nameScene);
        _myProfile.transform.GetChild(0).position.Set(0, 0, 0);
        _myCourses.transform.GetChild(0).DOLocalMoveX(0, 0);
        _setting.transform.GetChild(0).DOLocalMoveX(0, 0);
        _exitProfile.transform.GetChild(0).DOLocalMoveX(0, 0);
        _allCourses.transform.GetChild(0).DOLocalMoveX(0, 0);
        _message.SetActive(false);
        _menuTransform.DOBlendableLocalMoveBy(Vector2.right * -408, .5f);
        _menuWindow.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (!_menuWindow.activeSelf)
            {
                _menuWindow.SetActive(true);
                _menuTransform.DOBlendableLocalMoveBy(Vector2.right * 408, .5f);
            }
            else
            {
                _menuTransform.DOBlendableLocalMoveBy(Vector2.right * -408, .5f).OnComplete(() => _menuWindow.SetActive(false));
                _message.SetActive(false);
            }
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _myProfile?.SetActive(!(scene.name == "MainScene"));
        _allCourses?.SetActive(!(scene.name == "MainScene"));
        _myCourses?.SetActive(!(scene.name == "MainScene"));
        _setting?.SetActive(!(scene.name == "MainScene"));
        _exitProfile?.SetActive(!(scene.name == "MainScene"));
    }

    public void OnPointerEnter(GameObject gameObject)
    {
        gameObject.transform.DOLocalMoveX(40, .5f);
    }

    public void OnPointerExit(GameObject gameObject)
    {
        gameObject.transform.DOLocalMoveX(0, .5f);
    }

}
