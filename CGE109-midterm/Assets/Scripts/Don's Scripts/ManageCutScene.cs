using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class ManageCutScene : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    [SerializeField] private string nextSceneName;
    [SerializeField] private float holdESC = 2f;

    private float currentHoldTime = 0f;

    void Start()
    {
        Cursor.visible = false;
        if (videoPlayer == null)
        {
            var obj = GameObject.Find("VideoDisplay");
            if (obj != null)
            {
                videoPlayer = obj.GetComponent<VideoPlayer>();
                videoPlayer.loopPointReached += VideoFinished;
            }
        }
        else
        {
            Debug.Log("Can't find VideoDisplay");
        }
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))   // กด ESC ค้างไว้?
        {
            currentHoldTime += Time.deltaTime;

            // ถ้าค้างครบกำหนด → เปลี่ยนซีน
            if (currentHoldTime >= holdESC)
            {
                SceneManager.LoadScene(nextSceneName);
            }
        }
        else
        {
            // ถ้าปล่อยปุ่ม รีเซ็ตเวลา
            currentHoldTime = 0f;
        }
    }

    void VideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName);
    }
}

