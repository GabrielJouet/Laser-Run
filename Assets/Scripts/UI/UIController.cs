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
    /// Game ober screen that will be displayed at the end of the level.
    /// </summary>
    [SerializeField]
    private GameObject _gameOverScreen;

    /// <summary>
    /// Game ober screen that will be displayed at the end of the level if won.
    /// </summary>
    [SerializeField]
    private GameObject _winScreen;

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
        StartCoroutine(DelayScreenDisplay(win ? _winScreen : _gameOverScreen));
    }


    /// <summary>
    /// Coroutine used to delay the game over screen displays.
    /// </summary>
    /// <param name="screen">The screen to display</param>
    private IEnumerator DelayScreenDisplay(GameObject screen)
    {
        yield return new WaitForSeconds(0.5f);
        screen.SetActive(true);
    }
}