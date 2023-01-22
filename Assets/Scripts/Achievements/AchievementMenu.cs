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
        List<AchievementProgress> achievementProgress = Controller.Instance.SaveController.SaveFile.AchievementsProgress;
        SaveFile saveFile = Controller.Instance.SaveController.SaveFile;
        AchievementProgress progressBuffered = null;

        for (int i = 0; i < achievements.Count; i ++)
        {
            progressBuffered = achievementProgress.Find(x => x.ID == achievements[i].ID);
            Instantiate(_achievementPrefab, transform).Initialize(achievements[i], !saveFile.AchievementsUnlocked.Contains(achievements[i].ID), progressBuffered);
        }

        _achievementSlider.maxValue = achievements.Count;
        _achievementSlider.value = saveFile.AchievementsUnlocked.Count;

        _achievementCount.text = saveFile.AchievementsUnlocked.Count + " / " + achievements.Count + " achievements";
    }
}