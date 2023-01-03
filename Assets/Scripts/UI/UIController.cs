using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Class that will handle every UI interaction in game.
/// </summary>
public class UIController : MonoBehaviour
{
    /// <summary>
    /// How much time is left text component?
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI _timeLeft;

    /// <summary>
    /// Threat description text component?
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI _threatDescription;

    /// <summary>
    /// Caution text component?
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI _cautionText;

    /// <summary>
    /// Game over screen that will be displayed at the end of the level.
    /// </summary>
    [SerializeField]
    private GameObject _gameOverScreen;

    /// <summary>
    /// Game over text component.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI _gameOverText;

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


    /// <summary>
    /// Method called when the level has an extra threat level?
    /// </summary>
    /// <param name="description">The description of this threat</param>
    /// <param name="warning">Does this threat requires a warning?</param>
    public void UpdateThreatLevel(string description, bool warning)
    {
        _threatDescription.text = description;

        if (warning)
            StartCoroutine(DelayCautionText());

        StartCoroutine(DelayThreatLevel());
    }


    /// <summary>
    /// Coroutine used to delay caution text.
    /// </summary>
    private IEnumerator DelayCautionText()
    {
        _cautionText.enabled = true;
        yield return new WaitForSeconds(1);
        _cautionText.enabled = false;
    }


    /// <summary>
    /// Coroutine used to delay threat text.
    /// </summary>
    private IEnumerator DelayThreatLevel()
    {
        _threatDescription.enabled = true;
        yield return new WaitForSeconds(1);
        _threatDescription.enabled = false;
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