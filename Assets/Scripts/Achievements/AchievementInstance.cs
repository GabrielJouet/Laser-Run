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
    /// Method called at initialization.
    /// </summary>
    /// <param name="achievement">The achievement initialized</param>
    /// <param name="locked">Does this achievement is locked?</param>
    public void Initialize(Achievement achievement, bool locked)
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
    }
}