using Gamekit3D;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    private int CurrentScene = 0;
    [HideInInspector]public int GamePlayerCurrentScene = 0;
    private ChangeScene ChangeSceneScript;
    private void Awake()
    {
        ChangeSceneScript = FindObjectOfType<ChangeScene>();
    }
    private void Update()
    {
        CurrentScene = SceneManager.GetActiveScene().buildIndex;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(PauseGame());
        }
    }

    #region PauseGame
    public void Resume(string Type)
    {
        if (Type == "Resume")
        {
            Cursor.lockState = CursorLockMode.Locked;
            StartCoroutine(PauseGame());
        }else if (Type == "Manu")
        {
            Debug.Log("HERE");
            StartCoroutine(PauseGame());
        }
    }

    public void SetCanvasSortingOrder(Canvas Canvas, int Number)
    {
        Canvas.sortingOrder = Number;
    }

    public IEnumerator PauseGame()
    {
        if (ScreenFader.Instance.PauseGameCanvasGroup.alpha > 0.1f)
        {
            SetCanvasSortingOrder(ScreenFader.Instance.PauseGameCanvasGroup.GetComponent<Canvas>(), 0);
            FirstPersonController firstPersonController = FindObjectOfType<FirstPersonController>();
            firstPersonController.enabled = true;
            Time.timeScale = 1;
            StartCoroutine(ScreenFader.FadeSceneIn());
        }
        else //ขึ้นหน้าหยุด
        {
            SetCanvasSortingOrder (ScreenFader.Instance.PauseGameCanvasGroup.GetComponent<Canvas>(), 10);
            Cursor.lockState = CursorLockMode.None;
            FirstPersonController firstPersonController = FindObjectOfType<FirstPersonController>();
            firstPersonController.enabled = false;
            Time.timeScale = 0f;
            StartCoroutine(ScreenFader.FadeSceneOut(ScreenFader.FadeType.PauseGame));
        } 
        yield break;
    }
    #endregion

    public void ChangeScene(int number = 0)
    {
        GamePlayerCurrentScene = CurrentScene;
        ChangeSceneScript.ChangeSceneTo(number);
    }
}
