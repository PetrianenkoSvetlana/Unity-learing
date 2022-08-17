using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Course
{
    public string title;
    public string description;
    public string pathIcon;
    public List<Lesson> lessons;
    [NonSerialized]
    public Sprite icon;

    public override string ToString()
    {
        return string.Format("{0}, {1}, {2}, {3}", title, lessons, icon, lessons);
    }
}

[Serializable]
public class Lesson
{
    public string title;
    public string description;
    public string url;
    public override string ToString()
    {
        return string.Format("{0}, {1}, {2}", title, description, url);
    }
}