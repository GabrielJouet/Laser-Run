using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that will handle every inputs in main menu.
/// </summary>
public class MainMenuController : MonoBehaviour
{
    /// <summary>
    /// Confirmation game object.
    /// </summary>
    [SerializeField]
    private GameObject _firstConfirmation;

    /// <summary>
    /// Second confirmation game object.
    /// </summary>
    [SerializeField]
    private GameObject _secondConfirmation;

    /// <summary>
    /// Done confirmation game object.
    /// </summary>
    [SerializeField]
    private GameObject _doneConfirmation;


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


    /// <summary>
    /// Method called when we want to reset game save, this is the first confirmation.
    /// </summary>
    public void FirstReset()
    {
        _firstConfirmation.SetActive(true);
    }


    /// <summary>
    /// Method called when we want to reset game save, this is the second confirmation.
    /// </summary>
    public void SecondReset()
    {
        _secondConfirmation.SetActive(true);
    }


    /// <summary>
    /// Method called when we want to reset game save, this is the last confirmation.
    /// </summary>
    public void ResetSave()
    {
        Controller.Instance.SaveController.ResetData();

        _doneConfirmation.SetActive(true);
    }
}