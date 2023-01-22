using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class used to display a single achievement.
/// </summary>
public class AchievementInstance : MonoBehaviour
{
    /// <summary>
    /// The achievement icon component.
    /// </summary>
    [SerializeField]
    private Image _icon;

    /// <summary>
    /// The achievement title component.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI _title;

    /// <summary>
    /// The achievement description component.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI _description;


    /// <summary>
    /// Slider used to show achievement progress in a progress achievement.
    /// </summary>
    [SerializeField]
    private Slider _achievementProgress;



    /// <summary>
    /// Method called at initialization.
    /// </summary>
    /// <param name="achievement">The achievement initialized</param>
    /// <param name="locked">Does this achievement is locked?</param>
    /// <param name="progress">Does this achievement has a progress attached?</param>
    public void Initialize(Achievement achievement, bool locked, AchievementProgress progress)
    {
        if (locked && !achievement.Hintable)
        {
            _icon.sprite = achievement.LockedIcon;
            _title.text = "???";
            _description.text = "???";
        }
        else
        {
            _icon.sprite = locked ? achievement.LockedIcon : achievement.Icon;
            _title.text = achievement.Name;
            _description.text = locked ? "???" : achievement.Description;
        }

        if (achievement.Goal != 0)
        {
            _achievementProgress.gameObject.SetActive(true);
            _achievementProgress.maxValue = achievement.Goal;

            _achievementProgress.value = progress != null ? progress.Progress : 0;
        }
    }
}