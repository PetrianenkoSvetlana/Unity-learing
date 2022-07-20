using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

[Serializable]
public class Profile
{
    public string nameIcon;
    public string nameProfile;
    public string passwordProfile;

    public Profile(string nameIcon, string nameProfile, string passwordProfile)
    {
        this.nameIcon = nameIcon;
        this.nameProfile = nameProfile;

        var hash = new Hash128();
        hash.Append(passwordProfile);
        this.passwordProfile =  hash.ToString();
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

    public GameObject icon;
    public GameObject inputName;
    public GameObject inputPassword;
    public GameObject warningText;
    public Sprite[] icons;

    private InputField textName;
    private InputField textPassword;
    private Image textImage;

    private string pathSaveData;
    private int index = 0;
    private Profiles profiles = new Profiles();

    void Start()
    {
        pathSaveData = Application.persistentDataPath + "/SaveData.dat";
        textImage = icon.GetComponent<Image>();
        textName = inputName.GetComponentInChildren<InputField>();
        textPassword = inputPassword.GetComponent<InputField>();

        if (File.Exists(pathSaveData))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(pathSaveData, FileMode.Open);
            profiles = (Profiles)bf.Deserialize(file);
            file.Close();
        }

        iconGameObject.transform.GetChild(1).GetComponent<Image>().sprite = icons[index];
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
            profiles.profiles.Add(new Profile(icons[index].name, textName.text, textPassword.text));
            FileStream file = File.Open(pathSaveData, FileMode.OpenOrCreate);
            bf.Serialize(file, profiles);
            file.Close();
            SceneManager.LoadScene("SampleScene");
        }
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
