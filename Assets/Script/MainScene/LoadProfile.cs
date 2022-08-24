using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LoadProfile : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabProfile;
    [SerializeField]
    private GameObject content;
    [SerializeField]
    private GameObject windowAddProfile;
    [SerializeField]
    private GameObject windowAuthProfile;

    [SerializeField]
    private ObjectProfiles objectProfiles;

    private AuthProfile authProfile;

    private void Awake()
    {
        authProfile = windowAuthProfile.GetComponentInChildren<AuthProfile>();
        LoadData();
    }
    private void LoadData()
    {
        objectProfiles.LoadData();
        //foreach (Transform child in content.transform)
        //    Destroy(child.gameObject);
        //print(objectProfiles.Profiles);
        foreach (var profile in objectProfiles.Profiles)
        {
            var profileObj = Instantiate(prefabProfile);
            Sprite iconProfile = Resources.Load<Sprite>("Icons/" + profile.Icon);
            if (iconProfile.IsUnityNull())
            {
                StartCoroutine(LoadTextureFromServer(profile.Icon, profileObj, profile));
            }
            else
            {
                SetSprite(profileObj, iconProfile, profile);
            }
            profileObj.GetComponentInChildren<Text>().text = profile.Name;
            profileObj.transform.SetParent(content.transform, false);
        }
        StartCoroutine(ChangeSize());
    }

    //public void ActiveDeleteWindow(string name)
    //{
    //    warningText.SetActive(false);
    //    passwordInput.GetComponent<InputField>().text = "";
    //    deleteWindow.SetActive(true);
    //    deleteButton.GetComponent<Button>().onClick.RemoveAllListeners();
    //    deleteButton.GetComponent<Button>().onClick.AddListener(() => DeleteProfile(name));
    //}

    //public void DeleteProfile(string name)
    //{
    //    var password = passwordInput.GetComponent<InputField>().text;
    //    var hash = new Hash128();
    //    hash.Append(password);

    //    if (hash.ToString() == data.profiles[int.Parse(name)].password)
    //    {
    //        data.profiles.RemoveAt(int.Parse(name));
    //        BinaryFormatter bf = new BinaryFormatter();
    //        FileStream file = File.Open(pathSaveData, FileMode.OpenOrCreate);
    //        bf.Serialize(file, data);
    //        file.Close();
    //        LoadData();
    //        deleteWindow.SetActive(false);
    //    }
    //    else
    //    {
    //        warningText.SetActive(true);
    //        passwordInput.GetComponent<InputField>().text = "";
    //    }

    //}
    IEnumerator LoadTextureFromServer(string url, GameObject profileObj, Profile profile)
    {
        var request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();
        var texture = DownloadHandlerTexture.GetContent(request);
        var sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        SetSprite(profileObj, sprite, profile);
        request.Dispose();
    }
    private void SetSprite(GameObject profileObj, Sprite sprite, Profile profile)
    {
        profileObj.transform.GetChild(0).GetComponent<Image>().sprite = sprite;
        profileObj.GetComponentInChildren<Button>().onClick.AddListener(() => Auth(profile, sprite));
    }

    private void Auth(Profile profile, Sprite sprite)
    {
        authProfile.profile = profile;
        authProfile.icon = sprite;
        windowAuthProfile.SetActive(true);
    }

    IEnumerator ChangeSize()
    {
        yield return new WaitForEndOfFrame();

        Canvas.ForceUpdateCanvases();
        float heigthContent = content.GetComponent<RectTransform>().sizeDelta.y;
        RectTransform rectList = content.transform.parent.parent.GetComponent<RectTransform>();
        Vector2 size = rectList.sizeDelta;
        rectList.sizeDelta = heigthContent > 400f ? new Vector2(size.x, 400f) : new Vector2(size.x, heigthContent);
    }
}


