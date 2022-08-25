using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AuthProfile : MonoBehaviour
{
    [SerializeField] private GameObject inputPassword;
    [SerializeField] private int _vibrato;
    [SerializeField] private int _elasticity;

    [HideInInspector] public Profile profile;

    private InputField textPassword;

    //private void OnValidate()
    //{
    //    _elasticity = _elasticity < 0 ? 0 : _elasticity;
    //    _elasticity = _elasticity > 1 ? 1 : _elasticity;
    //}

    private void OnEnable()
    {
        textPassword = inputPassword.GetComponentInChildren<InputField>();
        textPassword.text = "";
        transform.GetChild(0).DOPunchScale(Vector3.one / 2, .5f, 1, 0);
    }

    public void Auth()
    {
        var hash = new Hash128();
        hash.Append(textPassword.text);
        bool error = false;

        error |= inputPassword.GetComponent<CheckingInput>().Checking(hash.ToString() == profile.Password);
        if (!error)
        {
            CurrentProfile.icon = profile.Icon;
            CurrentProfile.name = profile.Name;
            CurrentProfile.password = profile.Password;
            CurrentProfile.email = profile.Email;
            CurrentProfile.path = profile.Path;
            CurrentProfile.courses = profile.Courses;
            SceneManager.LoadScene("ProfileCourses");
        }

    }
}
