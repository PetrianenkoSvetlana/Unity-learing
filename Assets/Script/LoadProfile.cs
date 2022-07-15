using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadProfile : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabProfile;
    public Sprite[] listIcons;

    public int countProfile = 5;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < countProfile; i++)
        {
            var profile = GameObject.Instantiate(prefabProfile);
            profile.transform.GetChild(0).GetComponent<Image>().sprite = listIcons[Random.Range(0, 4)];
            profile.GetComponentInChildren<Text>().text = $"{i + 1} пользователь";
            profile.transform.SetParent(transform, false);
        }

        Canvas.ForceUpdateCanvases();

        float heigthContent =  gameObject.GetComponent<RectTransform>().sizeDelta.y;
        Vector2 size = transform.parent.parent.GetComponent<RectTransform>().sizeDelta;
        size = heigthContent > 400f ? new Vector2(size.x, 400f) : new Vector2(size.x, heigthContent);
        transform.parent.parent.GetComponent<RectTransform>().sizeDelta = size;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
