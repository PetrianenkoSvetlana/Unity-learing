using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class ObjectProfiles : MonoBehaviour
{
    private string pathSaveData;
    private List<Profile> profiles;

    public List<Profile> Profiles
    {
        get => profiles;
    }


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        pathSaveData = Application.persistentDataPath + "/SaveData.dat";
        LoadData();
    }
    /// <summary>
    /// Загрузка данных
    /// </summary>
    public void LoadData()
    {
        if (File.Exists(pathSaveData))
            using (FileStream file = File.Open(pathSaveData, FileMode.Open))
            {
                BinaryFormatter bf = new BinaryFormatter();
                profiles = (List<Profile>)bf.Deserialize(file);
                file.Close();
            }
    }
    /// <summary>
    /// Добавляем профиль
    /// </summary>
    /// <param name="profile"></param>
    public void AddProfile(Profile profile)
    {
        profiles.Add(profile);
        SaveData();
    }
    /// <summary>
    /// Сохранение данных
    /// </summary>
    public void SaveData()
    {
        using (FileStream file = File.Open(pathSaveData, FileMode.OpenOrCreate))
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, profiles);
            file.Close();
        }
    }
    public void AddCourse(Course course)
    {
        var profile = profiles.Find(x => x.Path == CurrentProfile.path);
        profile.Courses.Add(new MyCourse(course));
        SaveData();
    }
}
