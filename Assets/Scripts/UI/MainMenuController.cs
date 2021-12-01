using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that will handle every inputs in main menu.
/// </summary>
public class MainMenuController : MonoBehaviour
{
    /// <summary>
    /// Method called to open level selection screen.
    /// </summary>
    public void LoadLevelSelection()
    {
        SceneManager.LoadScene("LevelSelection");
    }


    /// <summary>
    /// Method called when we want to close the game.
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
}