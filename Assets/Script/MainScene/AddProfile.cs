using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;
using UnityEngine.Networking;
using SFB;
using System.Text.RegularExpressions;

public class AddProfile : MonoBehaviour
{
    [Header("Full Input")]
    public GameObject inputName;
    public GameObject inputEmail;

    [Header("Input")]
    public GameObject inputPassword;
    public GameObject inputPath;

    [Space(10f)]
    [SerializeField] private ObjectProfiles objectProfiles;
    public Sprite[] icons;
    [SerializeField] private GameObject iconGameObject;

    private InputField textName;
    private InputField textEmail;
    private InputField textPassword;
    private InputField textPath;

    private int index = 0;

    private void OnEnable()
    {
        textName = inputName.GetComponentInChildren<InputField>();
        textEmail = inputEmail.GetComponentInChildren<InputField>();

        textPassword = inputPassword.GetComponent<InputField>();
        textPath = inputPath.GetComponent<InputField>();
    
        textPath.text = Application.streamingAssetsPath;

        transform.GetChild(0).DOPunchScale(Vector3.one / 2, .5f, 1, 0);
    }

    /// <summary>
    /// Добавить пользователя
    /// </summary>
    public void Add()
    {
        bool error = false;

        error |= inputName.GetComponent<CheckingInput>().Checking(textName.text != "");

        bool isEmail = Regex.IsMatch(textEmail.text, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        error |= inputEmail.GetComponent<CheckingInput>().Checking(isEmail);

        if (!error)
        {
            Profile newProfile = new Profile(icons[index].name, textName.text, textPassword.text, textEmail.text, textPath.text);
            objectProfiles.AddProfile(newProfile);

            CurrentProfile.icon = icons[index];
            CurrentProfile.name = newProfile.Name;
            CurrentProfile.password = newProfile.Password;
            CurrentProfile.email = newProfile.Email;
            CurrentProfile.path = newProfile.Path;
            CurrentProfile.courses = new List<MyCourse>();

            if (Directory.Exists(CurrentProfile.path))
            {
                int number = 2;
                while (Directory.Exists(CurrentProfile.path + $" ({number})"))
                    number++;
                Directory.CreateDirectory(CurrentProfile.path + $" ({number})");
            }
            else
                Directory.CreateDirectory(CurrentProfile.path);
            SceneManager.LoadScene("AllСourses");
        }
    }

    /// <summary>
    /// Функция для добавления пути для сохранения проектов
    /// </summary>
    public void AddPath()
    {
        var path = StandaloneFileBrowser.OpenFolderPanel("Выберить путь для сохрания ваших проектов", textPath.text, false);
        textPath.text = path.Length != 0 ? path[0] : textPath.text;
    }


    /// <summary>
    /// Функция для добавления пути иконки пользователя
    /// </summary>
    public void AddPathIcon()
    {
        if (index < icons.Length - 1)
            return;
        var extensions = new[] {
            new ExtensionFilter("Image Files", "png", "jpg", "jpeg" ),
            new ExtensionFilter("All Files", "*" ),
        };
        var pathIcon = StandaloneFileBrowser.OpenFilePanel("Выберите иконку для профиля", "", extensions, false);
        StartCoroutine(LoadTextureFromServer(pathIcon, LoadTexture));
    }

    IEnumerator LoadTextureFromServer(string[] url, Action<Sprite> action)
    {
        if (url.Length != 0)
        {
            var request = UnityWebRequestTexture.GetTexture(url[0]);
            yield return request.SendWebRequest();
            var texture = DownloadHandlerTexture.GetContent(request);
            request.Dispose();

            Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
            sprite.name = url[0];

            action(sprite);
        }
    }

    private void LoadTexture(Sprite sprite)
    {
        icons[^1] = sprite;
        iconGameObject.transform.GetChild(0).GetComponent<Image>().sprite = icons.Last();
        iconGameObject.transform.GetChild(2).GetComponent<Image>().sprite = icons.Last();
    }

    public void LeftMove(int lengthShift)
    {
        iconGameObject.transform.localPosition = Vector3.zero;
        iconGameObject.transform.GetChild(1).GetComponent<Image>().sprite = icons[index];

        index = index == icons.Length - 1 ? 0 : index + 1;
        iconGameObject.transform.GetChild(2).GetComponent<Image>().sprite = icons[index];

        iconGameObject.transform.DOLocalMoveX(lengthShift, 0.5f).Restart();
    }

    public void RightMove(int lengthShift)
    {
        iconGameObject.transform.localPosition = Vector3.zero;
        iconGameObject.transform.GetChild(1).GetComponent<Image>().sprite = icons[index];

        index = index == 0 ? icons.Length - 1 : index - 1;
        iconGameObject.transform.GetChild(0).GetComponent<Image>().sprite = icons[index];

        iconGameObject.transform.DOLocalMoveX(lengthShift, 0.5f).Restart();
    }
}
