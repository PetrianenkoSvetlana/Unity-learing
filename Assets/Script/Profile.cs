using System;
using UnityEngine;

[Serializable]
public class Profile
{
    private string icon;
    private string name;
    private string password;
    private string email;
    private string path;


    public string Icon
    {
        get { return icon; }
        set { icon = value; }
    }
    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    public string Password
    {
        get { return password; }
        set { password = value; }
    }
    public string Email
    {
        get { return email; }
        set { email = value; }
    }
    public string Path
    {
        get { return path; }
        set { path = value; }
    }
    public Profile(string icon, string name, string password, string email, string path)
    {
        this.icon = icon;
        this.name = name;

        var hash = new Hash128();
        hash.Append(password);
        this.password = hash.ToString();
        this.email = email;
        this.path = path;
    }
}
