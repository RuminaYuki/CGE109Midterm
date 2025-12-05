using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void ChangeSceneTo(int Scene_Number)
    {
        SceneManager.LoadScene(Scene_Number);
    }
    public void GameStart(int Scene_Number)
    {
        Scene_Manager Scene_Manager = FindObjectOfType<Scene_Manager>();
        if (Scene_Manager != null && Scene_Manager.GamePlayerCurrentScene != 0 && Scene_Number == 1)
        {
            SceneManager.LoadScene(Scene_Manager.GamePlayerCurrentScene);
            return;
        }
        SceneManager.LoadScene(Scene_Number);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

}
