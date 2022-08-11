using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CourseScene : MonoBehaviour
{
    [SerializeField]
    private VideoPlayer vPlayer;
    [SerializeField]
    private GameObject windowLoading;

    private void Awake()
    {

    }

    public void PlayVideo(string url)
    {
        vPlayer.url = url;
        windowLoading.SetActive(true);
        vPlayer.Prepare();
    }

}
