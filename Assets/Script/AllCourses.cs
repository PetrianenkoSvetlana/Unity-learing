using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllCourses : MonoBehaviour
{
    [SerializeField]
    private GameObject icon;
    void Start()
    {
        icon.GetComponent<Image>().sprite = CurrentProfile.icon;
        icon.GetComponentInChildren<Text>().text = CurrentProfile.name;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
