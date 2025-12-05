using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public int SceneNumber;
    public void PlayGame(int Scene_Number)
    {
        SceneManager.LoadScene(Scene_Number);
    }




    public void QuitGame()
    {
        Application.Quit();
    }

}
