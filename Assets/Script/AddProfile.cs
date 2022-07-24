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

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public class OpenFileName
{
    public int structSize = 0;
    public IntPtr dlgOwner = IntPtr.Zero;
    public IntPtr instance = IntPtr.Zero;
    public String filter = null;
    public String customFilter = null;
    public int maxCustFilter = 0;
    public int filterIndex = 0;
    public String file = null;
    public int maxFile = 0;
    public String fileTitle = null;
    public int maxFileTitle = 0;
    public String initialDir = null;
    public String title = null;
    public int flags = 0;
    public short fileOffset = 0;
    public short fileExtension = 0;
    public String defExt = null;
    public IntPtr custData = IntPtr.Zero;
    public IntPtr hook = IntPtr.Zero;
    public String templateName = null;
    public IntPtr reservedPtr = IntPtr.Zero;
    public int reservedInt = 0;
    public int flagsEx = 0;
    public OpenFileName(int FileLenth = 256, int FileTitleLenth = 64)
    {
        structSize = Marshal.SizeOf(this);
        file = new string(new char[FileLenth]);
        maxFile = file.Length;
        fileTitle = new string(new char[FileTitleLenth]);
        maxFileTitle = fileTitle.Length;
        title = String.Empty;
        flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;
        title = "Заголовок окна";
        initialDir = Application.dataPath.Replace('/', '\\');
    }
}

public class LocalDialog
{
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetOpenFileName([In, Out] OpenFileName ofn);
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetSaveFileName([In, Out] OpenFileName ofn);
}

[Serializable]
public class Profile
{
    public string nameIcon;
    public string nameProfile;
    public string passwordProfile;
    public string pathProfile;

    public Profile(string nameIcon, string nameProfile, string passwordProfile, string pathProfile)
    {
        this.nameIcon = nameIcon;
        this.nameProfile = nameProfile;

        var hash = new Hash128();
        hash.Append(passwordProfile);
        this.passwordProfile = hash.ToString();
        this.pathProfile = pathProfile;
    }
}

[Serializable]
public class Profiles
{
    public List<Profile> profiles = new List<Profile>();
}

public class AddProfile : MonoBehaviour
{
    [SerializeField]
    private GameObject iconGameObject;

    //public GameObject icon;
    public GameObject inputName;
    public GameObject inputPassword;
    public GameObject inputPath;
    public GameObject warningText;
    public Sprite[] icons;

    private InputField textName;
    private InputField textPassword;
    private InputField textPath;

    private string pathSaveData;
    private int index = 0;
    private Profiles profiles = new Profiles();

    void Start()
    {
        pathSaveData = Application.persistentDataPath + "/SaveData.dat";
        textName = inputName.GetComponentInChildren<InputField>();
        textPassword = inputPassword.GetComponent<InputField>();
        textPath = inputPath.GetComponent<InputField>();

        if (File.Exists(pathSaveData))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(pathSaveData, FileMode.Open);
            profiles = (Profiles)bf.Deserialize(file);
            file.Close();
        }

        iconGameObject.transform.GetChild(1).GetComponent<Image>().sprite = icons[index];

        inputPath.transform.GetChild(0).GetComponent<Text>().text = Application.dataPath;
    }

    /// <summary>
    /// Добавить пользователя
    /// </summary>
    public void Add()
    {
        if (textName.text == "")
        {
            var rectTransf = inputName.GetComponent<RectTransform>();
            var sizeDelta = rectTransf.sizeDelta;
            rectTransf.sizeDelta = new Vector2(sizeDelta.x, 73);
            warningText.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform.GetChild(1).GetComponent<RectTransform>());
        }
        else
        {
            BinaryFormatter bf = new BinaryFormatter();
            profiles.profiles.Add(new Profile(icons[index].name, textName.text, textPassword.text, textPath.text));
            FileStream file = File.Open(pathSaveData, FileMode.OpenOrCreate);
            bf.Serialize(file, profiles);
            file.Close();
            CurrentProfile.nameIcon = profiles.profiles.Last().nameIcon;
            CurrentProfile.name = profiles.profiles.Last().nameProfile;
            CurrentProfile.password = profiles.profiles.Last().passwordProfile;
            CurrentProfile.path = profiles.profiles.Last().pathProfile;

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
        textPath.text = path.Length != 0 ? path[0] : "";

        //textPath.text = EditorUtility.OpenFolderPanel("Выберить путь", Application.dataPath, "");
    }

    IEnumerator LoadTextureFromServer(string[] url, Action<Texture2D> response)
    {
        if (url.Length != 0)
        {
            var request = UnityWebRequestTexture.GetTexture(url[0]);
            yield return request.SendWebRequest();
            response(DownloadHandlerTexture.GetContent(request));
            request.Dispose();
        }
        else
            response(null);

    }

    public void AddPathIcon()
    {
        if (icons[index].name != "default")
            return;
        //textPath.text = 
        var extensions = new[] {
            new ExtensionFilter("Image Files", "png", "jpg", "jpeg" ),
            new ExtensionFilter("All Files", "*" ),
        };
        var pathIcon = StandaloneFileBrowser.OpenFilePanel("Выберите иконку для профиля", "", extensions, false);
        //string pathIcon = EditorUtility.OpenFilePanelWithFilters("Выберить путь", "", new string[] { "Image files", "png,jpg,jpeg", "All files", "*" });
        StartCoroutine(LoadTextureFromServer(pathIcon, LoadTexture));

        //var texture = DownloadHandlerTexture.GetContent(UnityWebRequestTexture.GetTexture(pathIcon));

    }

    private void LoadTexture(Texture2D texture)
    {
        if (texture == null)
            return;
        var sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        sprite.name = "default";
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

    public void Back()
    {
        SceneManager.LoadScene("SampleScene");
    }


}
