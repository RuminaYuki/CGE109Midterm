using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoEndToScene : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextSceneName;

    void Start()
    {
        // ให้ฟังก์ชัน VideoFinished ทำงานเมื่อวิดีโอเล่นจบ
        videoPlayer.loopPointReached += VideoFinished;
    }

    void VideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName);
    }
}

