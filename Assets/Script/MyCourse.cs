using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class MyCourse : Course
{
    public MyCourse(Course course)
    {
        title = course.title;
        icon = course.icon;
        description = course.description;
        pathIcon = course.pathIcon;

        lessons = course.lessons.Select(p => new MyLesson(p)).ToList();
        lastLesson = lessons.First();
        //lessons = new List<MyLesson>((IEnumerable<MyLesson>)course.lessons);
    }

    //public string title;
    public new List<MyLesson> lessons;
    public MyLesson lastLesson;
    public bool finish = false;

    public override string ToString()
    {
        return string.Format("{0}, {1}, {2}, {3}", title, lessons, lastLesson, finish);
    }
}

[Serializable]
public class MyLesson : Lesson
{
    public MyLesson(Lesson lesson)
    {
        title = lesson.title;
        description = lesson.description;
        url = lesson.url;
    }

    //public string title;
    //public string description;
    //public string url;
    public bool finish = false;
    
    public override string ToString()
    {
        return string.Format("{0}, {1}, {2}, {3}", title, description, url, finish);
    }
}
