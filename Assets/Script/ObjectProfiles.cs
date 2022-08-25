using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/Create Data", order = 51)]
public class ObjectProfiles : ScriptableObject
{
    private string _pathSaveData;
    [SerializeField] private List<Profile> _profiles;

    public List<Profile> Profiles => _profiles;

    /// <summary>
    /// �������� ������
    /// </summary>
    public void LoadData()
    {
        _pathSaveData = Application.persistentDataPath + "/SaveData.dat";
        if (File.Exists(_pathSaveData))
            using (FileStream file = File.Open(_pathSaveData, FileMode.Open))
            {
                BinaryFormatter bf = new BinaryFormatter();
                _profiles = (List<Profile>)bf.Deserialize(file);
                file.Close();
            }
        else
        {
            _profiles = new List<Profile>();
        }
    }
    /// <summary>
    /// ��������� �������
    /// </summary>
    /// <param name="profile"></param>
    public void AddProfile(Profile profile)
    {
        _profiles.Add(profile);
        SaveData();
    }
    /// <summary>
    /// ���������� ������
    /// </summary>
    public void SaveData()
    {
        using (FileStream file = File.Open(_pathSaveData, FileMode.OpenOrCreate))
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, _profiles);
            file.Close();
        }
    }
    /// <summary>
    /// ���������� ����� ������������
    /// </summary>
    /// <param name="course">����</param>
    public void AddCourse(Course course)
    {
        var profile = _profiles.Find(x => x.Path == CurrentProfile.path);
        profile.Courses.Add(new MyCourse(course));
        SaveData();
    }

    /// <summary>
    /// ���������� ����� ������������
    /// </summary>
    /// <param name="course">����</param>
    public void AddCourse(MyCourse course)
    {
        var profile = _profiles.Find(x => x.Path == CurrentProfile.path);
        profile.Courses.Add(course);
        SaveData();
    }

    /// <summary>
    /// �������� ������������
    /// </summary>
    /// <param name="profile"></param>
    public void DeleteProfile(Profile profile)
    {
        _profiles.Remove(profile);
        SaveData();
    }
}
