using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LoadProfile : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabProfile;
    [SerializeField]
    private GameObject content;
    [SerializeField]
    private GameObject deleteWindow;
    [SerializeField]
    private GameObject deleteButton;
    [SerializeField]
    private GameObject passwordInput; 
    [SerializeField]
    private GameObject warningText;

    private Profiles data;
    private string pathSaveData;

    void Start()
    {

        pathSaveData = Application.persistentDataPath + "/SaveData.dat";
        LoadData();
    }

    public void LoadScene()
    {
        SceneManager.LoadScene("NewProfile");
    }

    public void ActiveDeleteWindow(string name)
    {
        warningText.SetActive(false);
        passwordInput.GetComponent<InputField>().text = "";
        deleteWindow.SetActive(true);
        deleteButton.GetComponent<Button>().onClick.RemoveAllListeners();
        deleteButton.GetComponent<Button>().onClick.AddListener(() => DeleteProfile(name));
    }

    public void UnactiveDeleteWindow()
    {
        deleteWindow.SetActive(false);
    }

    public void DeleteProfile(string name)
    {
        var password = passwordInput.GetComponent<InputField>().text;
        var hash = new Hash128();
        hash.Append(password);

        if (hash.ToString() == data.profiles[int.Parse(name)].passwordProfile)
        {
            data.profiles.RemoveAt(int.Parse(name));
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(pathSaveData, FileMode.OpenOrCreate);
            bf.Serialize(file, data);
            file.Close();
            LoadData();
            deleteWindow.SetActive(false);
        }
        else
        {
            warningText.SetActive(true);
            passwordInput.GetComponent<InputField>().text = "";
        }
        
    }

    private void LoadData()
    {
        if (File.Exists(pathSaveData))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(pathSaveData, FileMode.Open);
            data = (Profiles)bf.Deserialize(file);
            file.Close();

            foreach (Transform child in content.transform)
                Destroy(child.gameObject);

            int numberProfile = 0;

            foreach (var profile in data.profiles)
            {
                var profileObj = Instantiate(prefabProfile);
                profileObj.name = numberProfile.ToString();
                profileObj.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(profile.nameIcon);
                profileObj.GetComponentInChildren<Text>().text = profile.nameProfile;
                profileObj.transform.SetParent(content.transform, false);
                numberProfile++;
            }
        }
        StartCoroutine(ChangeSize());
    }

    IEnumerator ChangeSize()
    {
        yield return new WaitForEndOfFrame();

        Canvas.ForceUpdateCanvases();
        float heigthContent = content.GetComponent<RectTransform>().sizeDelta.y;
        Vector2 size = content.transform.parent.parent.GetComponent<RectTransform>().sizeDelta;
        size = heigthContent > 400f ? new Vector2(size.x, 400f) : new Vector2(size.x, heigthContent);
        content.transform.parent.parent.GetComponent<RectTransform>().sizeDelta = size;
    }

}
