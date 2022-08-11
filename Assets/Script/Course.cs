using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Course
{
    public string name;
    public string description;
    public Lesson[] lessons;
    public bool finish;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class Lesson
{
    public string name;
    public string text;
    //public string url;
    public bool finish;

}