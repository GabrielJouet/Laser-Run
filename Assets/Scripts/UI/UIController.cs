using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that will handle every UI interaction in game.
/// </summary>
public class UIController : MonoBehaviour
{
    /// <summary>
    /// How much time is left component?
    /// </summary>
    [SerializeField]
    private Text _timeLeft;

    [SerializeField]
    private Text _threatDescription;

    /// <summary>
    /// Game over screen that will be displayed at the end of the level.
    /// </summary>
    [SerializeField]
    private GameObject _gameOverScreen;

    /// <summary>
    /// Game over text component.
    /// </summary>
    [SerializeField]
    private Text _gameOverText;

    /// <summary>
    /// How much time between activation and display.
    /// </summary>
    [SerializeField]
    private float _screenDelayTime;
    public float ScreenDelayTime { get => _screenDelayTime; }


    /// <summary>
    /// Method called to update the slider and text component.
    /// </summary>
    /// <param name="timeLeft">The time left</param>
    public void UpdateTimeLeft(float timeLeft)
    {
        _timeLeft.text = string.Format("{0:#.00 sec}", timeLeft);
    }


    public void UpdateThreatLevel(string description)
    {
        _threatDescription.text = description;
    }


    /// <summary>
    /// Method used to display game over screen.
    /// </summary>
    /// <param name="win">Does the player wins the game?</param>
    public void DisplayGameOverScreen(bool win)
    {
        StartCoroutine(DelayScreenDisplay(win ? "Laser won't stop you this time!" : "You've stopped running..."));
    }


    /// <summary>
    /// Method used to hide the game over screen.
    /// </summary>
    public void HideGameOverScreen()
    {
        _gameOverScreen.SetActive(false);
    }


    /// <summary>
    /// Coroutine used to delay the game over screen displays.
    /// </summary>
    /// <param name="displayText">The text to display</param>
    private IEnumerator DelayScreenDisplay(string displayText)
    {
        yield return new WaitForSeconds(ScreenDelayTime);
        _gameOverScreen.SetActive(true);
        _gameOverText.text = displayText;
    }
}