using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AuthProfile : MonoBehaviour
{
    [HideInInspector]
    public Profile profile;
    [HideInInspector]
    public Sprite icon;

    [SerializeField]
    private GameObject inputPassword;


    private InputField textPassword;
    private void OnEnable()
    {
        textPassword = inputPassword.GetComponentInChildren<InputField>();
    }

    public void Auth()
    {
        var hash = new Hash128();
        hash.Append(textPassword.text);
        bool error = false;

        error |= inputPassword.GetComponent<CheckingInput>().Checking(hash.ToString() == profile.Password);
        if (!error)
        {
            CurrentProfile.icon = icon;
            CurrentProfile.name = profile.Name;
            CurrentProfile.password = profile.Password;
            CurrentProfile.email = profile.Email;
            CurrentProfile.path = profile.Path;
            SceneManager.LoadScene("ProfileCourses");
        }
    }
}
