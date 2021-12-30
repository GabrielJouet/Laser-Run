using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class used to handle a UI level state.
/// </summary>
public class LevelMenu : MonoBehaviour
{
    /// <summary>
    /// Time reached text component.
    /// </summary>
    [SerializeField]
    private Text _timeText;

    /// <summary>
    /// Image locked, disabled if unlocked.
    /// </summary>
    [SerializeField]
    private GameObject _lockedImage;

    /// <summary>
    /// Finished image, unlocks after beating the level.
    /// </summary>
    [SerializeField]
    private GameObject _finishedImage;

    /// <summary>
    /// Finished hard image, unlocks after beating the level in hard mode.
    /// </summary>
    [SerializeField]
    private GameObject _finishedHardImage;

    /// <summary>
    /// Hard button component.
    /// </summary>
    [SerializeField]
    private GameObject _hardButton;



    /// <summary>
    /// Initialize method, called to start the object.
    /// </summary>
    /// <param name="locked">Does this level is locked?</param>
    /// <param name="maxTime">Max time reached in this level</param>
    /// <param name="hard">Does the level was hard finished?</param>
    /// <param name="win">Does the level was finished?</param>
    public void Initialize(bool locked, float maxTime, bool hard, bool win)
    {
        _timeText.text = string.Format("{0:#.00 sec}", maxTime);

        _lockedImage.SetActive(locked);

        _finishedImage.SetActive(win);
        _finishedHardImage.SetActive(hard);

        _hardButton.SetActive(win);
    }
}