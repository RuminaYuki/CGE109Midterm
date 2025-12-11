using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string TitleScene;
    public void ChangeSceneTo(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
    public void GameStart(string SceneName)
    {
        Scene_Manager Scene_Manager = FindObjectOfType<Scene_Manager>();
        if (Scene_Manager != null && Scene_Manager.GamePlayerCurrentScene != 0 && SceneName == TitleScene)
        {
            SceneManager.LoadScene(Scene_Manager.GamePlayerCurrentScene);
            return;
        }
        SceneManager.LoadScene(SceneName);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

}
