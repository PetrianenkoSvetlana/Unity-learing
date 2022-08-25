using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Profile
{
    private string pathIcon;
    private string name;
    private string password;
    private string email;
    private string path;
    private List<MyCourse> courses;

    [NonSerialized] private Sprite icon;

    /// <summary>
    /// Путь к картинке на ПК
    /// </summary>
    public string PathIcon
    {
        get { return pathIcon; }
        set { pathIcon = value; }
    }

    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    /// <summary>
    /// Пароль пользователя (точнее хэш)
    /// </summary>
    public string Password
    {
        get { return password; }
        set { password = value; }
    }
    /// <summary>
    /// Email пользователя, для востановления пароля
    /// </summary>
    public string Email
    {
        get { return email; }
        set { email = value; }
    }

    /// <summary>
    /// Путь к папке пользователя, где хранятся все его файлы с курсамм
    /// </summary>
    public string Path
    {
        get { return path; }
        set { path = value; }
    }

    /// <summary>
    /// Иконка пользователя
    /// </summary>
    public Sprite Icon
    {
        get { return icon; }
        set { icon = value; }
    }

    /// <summary>
    /// Список курсов пользователя
    /// </summary>
    public List<MyCourse> Courses
    {
        get { return courses; }
        set { courses = value; }
    }
    public Profile(string pathIcon, string name, string password, string email, string path)
    {
        this.pathIcon = pathIcon;
        this.name = name;

        var hash = new Hash128();
        hash.Append(password);
        this.password = hash.ToString();
        this.email = email;
        this.path = System.IO.Path.Combine(path, name);
        courses = new List<MyCourse>();
    }
}
