using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowHidePassword : MonoBehaviour
{
    [SerializeField]
    private Sprite[] eye;
    public void OnClick()
    {
        var inputField = gameObject.GetComponent<InputField>();
        if (inputField.contentType == InputField.ContentType.Password)
        {
            inputField.contentType = InputField.ContentType.Standard;
            gameObject.transform.GetChild(3).GetComponent<Image>().sprite = eye[0];
        }
        else
        {
            inputField.contentType = InputField.ContentType.Password;
            gameObject.transform.GetChild(3).GetComponent<Image>().sprite = eye[1];
        }
        inputField.ForceLabelUpdate();
    }
}
