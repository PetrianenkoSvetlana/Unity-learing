public class MyCourse
{
    public string name;
    public MyLesson[] lessons;
    public MyLesson lastLesson;
    public bool finish = false;

    //public string LastLesson()
    //{
    //    foreach (var lesson in lessons)
    //    {
    //        if (!lesson.finish)
    //            return lesson.name;
    //    }
    //    return "";
    //}
}

public class MyLesson
{
    public string name;
    public string description;
    public bool finish = false;
}
