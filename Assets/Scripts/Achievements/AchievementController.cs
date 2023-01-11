using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class used to handle every achievement behavior.
/// </summary>
public class AchievementController : MonoBehaviour
{
    /// <summary>
    /// The achievement popup that will be used to display achievements.
    /// </summary>
    [SerializeField]
    private AchievementPopup _achievementPopup;


    /// <summary>
    /// All achievements.
    /// </summary>
    [SerializeField]
    private List<Achievement> _achievements;

    /// <summary>
    /// All achievements.
    /// </summary>
    public List<Achievement> Achievements { get => _achievements; }






    /// <summary>
    /// Method used to trigger an achievement on completion.
    /// </summary>
    /// <param name="achievement">The achievement triggered</param>
    public void TriggerAchievement(Achievement achievement)
    {
        if (!GetComponent<SaveController>().SaveFile.AchievementsUnlocked.Contains(achievement.ID))
        {
            GetComponent<SaveController>().SaveAchievement(achievement.ID);

            _achievementPopup.PopupAchievement(achievement);
        }
    }


    /// <summary>
    /// Method used to trigger an achievement on completion.
    /// </summary>
    /// <param name="achievementID">The achievement triggered id</param>
    public void TriggerAchievement(string achievementID)
    {
        Achievement achievement = Achievements.Find(x => x.ID == achievementID);

        TriggerAchievement(Achievements.Find(x => x.ID == achievementID));
    }
}