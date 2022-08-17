using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class CurrentProfile
{
    static public Sprite icon;
    static public string name;
    static public string password;
    static public string email;
    static public string path;
    static public List<MyCourse> courses = new List<MyCourse>();
    static public MyCourse currentCourse;
}
