using System;

/// <summary>
/// Class used to serialize achievement progress data.
/// </summary>
[Serializable]
public class AchievementProgress
{
    /// <summary>
    /// Unique id of this achievement.
    /// </summary>
    public string ID { get; private set; }


    /// <summary>
    /// Progress of this achievement in an int format.
    /// </summary>
    public int Progress { get; private set; }


    /// <summary>
    /// Progress goal of this achievement in an int format.
    /// </summary>
    public int ProgressGoal { get; private set; }



    /// <summary>
    /// Constructor of this class.
    /// </summary>
    /// <param name="id">Unique id of the achievement</param>
    /// <param name="progressGoal">Progress goal of this achievement</param>
    public AchievementProgress(string id, int progressGoal)
    {
        ID = id;
        Progress = 0;
        ProgressGoal = progressGoal;
    }

    
    /// <summary>
    /// Method used to update the progress of an achievement.
    /// </summary>
    /// <param name="newProgress">New progress of this achievement</param>
    public void UpdateProgress(int newProgress)
    {
        if (newProgress < ProgressGoal)
            Progress = newProgress;
        else
            Progress = ProgressGoal;
    }


    /// <summary>
    /// Method used to increase the value of progress of an achievement.
    /// </summary>
    /// <param name="newProgress">New progress of this achievement</param>
    public void IncreaseProgress(int newProgress)
    {
        if (Progress + newProgress < ProgressGoal)
            Progress += newProgress;
        else
            Progress = ProgressGoal;
    }
}