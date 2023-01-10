using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class used to handle achievement menu.
/// </summary>
public class AchievementMenu : MonoBehaviour
{
    /// <summary>
    /// Achievement prefab used in UI visualization.
    /// </summary>
    [SerializeField]
    private AchievementInstance _achievementPrefab;

    /// <summary>
    /// Achievement slider used to display missing achievements.
    /// </summary>
    [SerializeField]
    private Slider _achievementSlider;

    /// <summary>
    /// Achievement count text component used to display missing achievements.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI _achievementCount;



    /// <summary>
    /// Start method, called at initialization after Awake.
    /// </summary>
    private void Start()
    {
        List<Achievement> achievements = Controller.Instance.AchievementController.Achievements;
        SaveFile saveFile = Controller.Instance.SaveController.SaveFile;

        for (int i = 0; i < achievements.Count; i ++)
            Instantiate(_achievementPrefab, transform).Initialize(achievements[i], !saveFile.Achievements.Contains(achievements[i].ID));

        _achievementSlider.maxValue = achievements.Count;
        _achievementSlider.value = saveFile.Achievements.Count;

        _achievementCount.text = saveFile.Achievements.Count + " / " + achievements.Count + " achievements";
    }
}