using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void LoadLevelSelection()
    {
        SceneManager.LoadScene("LevelSelection");
    }


    public void ExitGame()
    {
        Application.Quit();
    }
}