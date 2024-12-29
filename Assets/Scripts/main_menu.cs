using UnityEngine;
using UnityEngine.SceneManagement;

public class main_menu : MonoBehaviour
{
    public void PlayScene()
    {
        SceneManager.LoadScene("Levels");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void DemoScene()
    {
        SceneManager.LoadScene("Playground");
    }
}
