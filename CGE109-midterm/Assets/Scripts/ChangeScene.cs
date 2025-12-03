using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string SceneString;
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneString);
    }




    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

}
