using TMPro;
using UnityEngine;

/// <summary>
/// Class used to handle a UI level state.
/// </summary>
public class LevelMenu : MonoBehaviour
{
    /// <summary>
    /// Time reached text component.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI _timeText;

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
    /// <param name="state">The saved level state</param>
    /// <param name="maxTime">Max time reached in this level</param>
    public void Initialize(LevelState state, float maxTime)
    {
        _timeText.text = (maxTime < 1 ? "0" : "") + string.Format("{0:#.00 sec}", maxTime);

        _lockedImage.SetActive(state == LevelState.LOCKED);

        _finishedImage.SetActive(state == LevelState.WON);
        _finishedHardImage.SetActive(state == LevelState.WONHARD);

        _hardButton.SetActive(state == LevelState.WON);
    }
}