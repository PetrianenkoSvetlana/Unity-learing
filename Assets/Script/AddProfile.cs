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
using UnityEditor;
using System.Runtime.InteropServices;
using UnityEngine.Networking;
using SFB;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;

[Serializable]
public class Profile
{
    public string icon;
    public string name;
    public string password;
    public string email;
    public string path;

    public Profile(string icon, string name, string password, string email, string path)
    {
        this.icon = icon;
        this.name = name;

        var hash = new Hash128();
        hash.Append(password);
        this.password = hash.ToString();
        this.email = email;
        this.path = path;
    }
}

[Serializable]
public class Profiles
{
    public List<Profile> profiles = new List<Profile>();
}

public class AddProfile : MonoBehaviour
{
    [Header("Full Input")]
    public GameObject inputName;
    public GameObject inputEmail;

    [Header("Input")]
    public GameObject inputPassword;
    public GameObject inputPath;

    [Space(10f)]
    public GameObject canvas;
    public Sprite[] icons;
    [SerializeField]
    private GameObject iconGameObject;

    private InputField textName;
    private InputField textEmail;
    private InputField textPassword;
    private InputField textPath;

    //private string pathSaveData;
    private int index = 0;
    private Profiles profiles;

    private void OnEnable()
    {
        profiles = canvas.GetComponent<LoadProfile>().data;

        textName = inputName.GetComponentInChildren<InputField>();
        textEmail = inputEmail.GetComponentInChildren<InputField>();

        textPassword = inputPassword.GetComponent<InputField>();
        textPath = inputPath.GetComponent<InputField>();
    
        textPath.text = Application.dataPath;
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

        //if (textName.text == "")
        //{
        //var rectTransf = inputName.GetComponent<RectTransform>();
        //var sizeDelta = rectTransf.sizeDelta;
        //rectTransf.sizeDelta = new Vector2(sizeDelta.x, 55);
        //warningText.SetActive(true);
        //LayoutRebuilder.ForceRebuildLayoutImmediate(transform.GetChild(1).GetComponent<RectTransform>());
        //}
        if (!error)
        {
            BinaryFormatter bf = new BinaryFormatter();
            profiles.profiles.Add(new Profile(icons[index].name, textName.text, textPassword.text, textEmail.text, textPath.text));
            FileStream file = File.Open(LoadProfile.pathSaveData, FileMode.OpenOrCreate);
            bf.Serialize(file, profiles);
            file.Close();
            CurrentProfile.icon = profiles.profiles.Last().icon;
            CurrentProfile.name = profiles.profiles.Last().name;
            CurrentProfile.password = profiles.profiles.Last().password;
            CurrentProfile.email = profiles.profiles.Last().email;
            CurrentProfile.path = profiles.profiles.Last().path;

            System.IO.Directory.CreateDirectory(CurrentProfile.path + "/" + CurrentProfile.name);

            SceneManager.LoadScene("AllСourses");
        }
    }

    public void AddPath()
    {

        //OpenFileName openFileName = new OpenFileName();
        //if (LocalDialog.GetOpenFileName(openFileName))
        //{
        //    Debug.Log(openFileName.file);
        //    Debug.Log(openFileName.fileTitle);
        //};
        var path = StandaloneFileBrowser.OpenFolderPanel("Выберить путь для сохрания ваших проектов", "", false);
        textPath.text = path.Length != 0 ? path[0] : textPath.text;

        //textPath.text = EditorUtility.OpenFolderPanel("Выберить путь", Application.dataPath, "");
    }

    IEnumerator LoadTextureFromServer(string[] url, Action<Texture2D> response)
    {
        if (url.Length != 0)
        {
            var request = UnityWebRequestTexture.GetTexture(url[0]);
            yield return request.SendWebRequest();
            var texture = DownloadHandlerTexture.GetContent(request);
            texture.name = url[0];
            response(texture);
            request.Dispose();
        }
        else
            response(null);

    }

    public void AddPathIcon()
    {
        if (index < icons.Length - 1)
            return;
        //textPath.text = 
        var extensions = new[] {
            new ExtensionFilter("Image Files", "png", "jpg", "jpeg" ),
            new ExtensionFilter("All Files", "*" ),
        };
        var pathIcon = StandaloneFileBrowser.OpenFilePanel("Выберите иконку для профиля", "", extensions, false);
        StartCoroutine(LoadTextureFromServer(pathIcon, LoadTexture));

    }

    private void LoadTexture(Texture2D texture)
    {
        if (texture == null)
            return;
        var sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        
        sprite.name = texture.name;
        icons[^1] = sprite;
        iconGameObject.transform.GetChild(0).GetComponent<Image>().sprite = icons.Last();
        iconGameObject.transform.GetChild(2).GetComponent<Image>().sprite = icons.Last();
    }

    public void LeftMove()
    {
        iconGameObject.transform.localPosition = Vector3.zero;
        iconGameObject.transform.GetChild(1).GetComponent<Image>().sprite = icons[index];

        index = index == icons.Length - 1 ? 0 : index + 1;
        iconGameObject.transform.GetChild(2).GetComponent<Image>().sprite = icons[index];

        iconGameObject.transform.DOLocalMoveX(-160, 0.5f).Restart();
    }

    public void RightMove()
    {
        iconGameObject.transform.localPosition = Vector3.zero;
        iconGameObject.transform.GetChild(1).GetComponent<Image>().sprite = icons[index];

        index = index == 0 ? icons.Length - 1 : index - 1;
        iconGameObject.transform.GetChild(0).GetComponent<Image>().sprite = icons[index];

        iconGameObject.transform.DOLocalMoveX(160, 0.5f).Restart();
    }

    public void UnactiveWindow()
    {
        //RectTransform[] rectTransforms = GetComponentsInChildren<RectTransform>();
        //foreach (RectTransform rectTransform in rectTransforms)
        //{
        //    if (EventSystem.current.currentSelectedGameObject == rectTransform.gameObject)
        //    {
        //        gameObject.SetActive(false);
        //    };
        //}
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (EventSystem.current.currentSelectedGameObject == rectTransform.gameObject)
        {
            gameObject.SetActive(false);
        };
    }
}
