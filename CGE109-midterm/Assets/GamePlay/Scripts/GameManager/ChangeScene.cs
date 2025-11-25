using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public int SceneNumber;
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneNumber);
    }




    public void QuitGame()
    {
        Application.Quit();
    }

}
