using UnityEngine;
using UnityEngine.Video;
using System.IO;

public class PlayVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        string path = Path.Combine(
            Application.streamingAssetsPath,
            "CGE109-introGame.mp4"
        );

        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = path;

        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += OnPrepared;
    }

    void OnPrepared(VideoPlayer vp)
    {
        vp.Play();
    }
}
