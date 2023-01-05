using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class used to handle a UI level state.
/// </summary>
public class LevelMenu : MonoBehaviour
{
    /// <summary>
    /// Name text component.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI _nameLevel;

    /// <summary>
    /// Time reached text component.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI _timeText;

    /// <summary>
    /// Time reached text component in hard mode.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI _hardTimeText;

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
    /// Normal button component.
    /// </summary>
    [SerializeField]
    private Button _normalButton;

    /// <summary>
    /// Hard button component.
    /// </summary>
    [SerializeField]
    private Button _hardButton;

    /// <summary>
    /// Colors used depending of the category.
    /// </summary>
    [SerializeField]
    private List<Color> _bordersColors;



    /// <summary>
    /// Initialize method, called to start the object.
    /// </summary>
    /// <param name="save">The saved level</param>
    /// <param name="levelName">The level name used</param>
    /// <param name="controller">Parent controller of this button</param>
    /// <param name="index">Index of this level</param>
    /// <param name="category">Category of this level</param>
    public void Initialize(LevelSave save, string levelName, LevelSelectionController controller, int index, LevelCategory category)
    {
        GetComponent<Image>().color = _bordersColors[(int)category];
        _nameLevel.text = levelName;

        _timeText.text = (save.Time < 1 ? "0" : "") + string.Format("{0:#.00 sec}", save.Time);
        _hardTimeText.text = (save.HardTime < 1 ? "0" : "") + string.Format("{0:#.00 sec}", save.HardTime);

        _lockedImage.SetActive(save.State == LevelState.LOCKED);

        _finishedImage.SetActive(save.State == LevelState.WON);
        _finishedHardImage.SetActive(save.State == LevelState.WONHARD);

        _hardButton.gameObject.SetActive(save.State == LevelState.WON);
        _hardTimeText.gameObject.SetActive(save.State == LevelState.WON);

        _normalButton.onClick.AddListener(() => controller.LoadLevel(index));
        _hardButton.onClick.AddListener(() => controller.LoadHardLevel(index));
    }
}