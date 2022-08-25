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
    [SerializeField] private GameObject _prefabProfile;
    [SerializeField] private GameObject _content;
    [SerializeField] private GameObject _windowAddProfile;
    [SerializeField] private GameObject _windowAuthProfile;
    [SerializeField] private GameObject _menuWindow;

    [SerializeField] private ObjectProfiles _dataProfiles;

    private AuthProfile authProfile;

    private void Awake()
    {
        authProfile = _windowAuthProfile.GetComponentInChildren<AuthProfile>();
        LoadData();
    }
    private void LoadData()
    {
        _dataProfiles.LoadData();
        //foreach (Transform child in content.transform)
        //    Destroy(child.gameObject);
        foreach (var profile in _dataProfiles.Profiles)
        {
            var profileObj = Instantiate(_prefabProfile);
            Sprite iconProfile = Resources.Load<Sprite>("Icons/" + profile.PathIcon);
            if (iconProfile.IsUnityNull())
            {
                StartCoroutine(LoadTexture(profileObj, profile));
            }
            else
            {
                profile.Icon = iconProfile;
                SetSprite(profileObj, profile);
            }
            profileObj.GetComponentInChildren<Text>().text = profile.Name;
            profileObj.transform.SetParent(_content.transform, false);
        }
        StartCoroutine(ChangeSize());
    }
    IEnumerator LoadTexture(GameObject profileObj, Profile profile)
    {
        var request = UnityWebRequestTexture.GetTexture(profile.PathIcon);
        yield return request.SendWebRequest();
        var texture = DownloadHandlerTexture.GetContent(request);
        var sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        profile.Icon = sprite;
        SetSprite(profileObj, profile);
        request.Dispose();
    }
    private void SetSprite(GameObject profileObj, Profile profile)
    {
        profileObj.transform.GetChild(0).GetComponent<Image>().sprite = profile.Icon;
        profileObj.GetComponentInChildren<Button>().onClick.AddListener(() => Auth(profile));
    }

    private void Auth(Profile profile)
    {
        authProfile.profile = profile;
        _windowAuthProfile.SetActive(true);
    }

    IEnumerator ChangeSize()
    {
        yield return new WaitForEndOfFrame();

        Canvas.ForceUpdateCanvases();
        float heigthContent = _content.GetComponent<RectTransform>().sizeDelta.y;
        RectTransform rectList = _content.transform.parent.parent.GetComponent<RectTransform>();
        Vector2 size = rectList.sizeDelta;
        rectList.sizeDelta = heigthContent > 400f ? new Vector2(size.x, 400f) : new Vector2(size.x, heigthContent);
    }
}


