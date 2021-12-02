using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that will handle every UI interaction in game.
/// </summary>
public class UIController : MonoBehaviour
{
    /// <summary>
    /// The score slider of remaining time.
    /// </summary>
    [SerializeField]
    private Slider _scoreSlider;

    /// <summary>
    /// How much time is left component?
    /// </summary>
    [SerializeField]
    private Text _timeLeft;

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
    /// Time max of this level.
    /// </summary>
    private float _timeMax;


    /// <summary>
    /// Method used to set the time max.
    /// </summary>
    /// <param name="timeMax">The new value for time max</param>
    public void SetTimeMax(float timeMax)
    {
        _timeMax = timeMax;
    }


    /// <summary>
    /// Method called to update the slider and text component.
    /// </summary>
    /// <param name="timeElapsed">The time elapsed</param>
    public void UpdateTimeLeft(float timeElapsed)
    {
        _scoreSlider.value = timeElapsed / _timeMax;
        _timeLeft.text = string.Format("{0:#.00 sec}", _timeMax - timeElapsed);
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